using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Offline_Streamer
{
    public partial class Downloads : System.Web.UI.Page
    {
        private const string TokenEndpoint = "https://accounts.spotify.com/api/token";
        private const string SavedTracksEndpoint = "https://api.spotify.com/v1/me/tracks";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var code = Request.QueryString["code"];
                var state = Request.QueryString["state"];
                var error = Request.QueryString["error"];

                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException("Spotify OAuth returned error: " + HttpUtility.HtmlEncode(error));
                }

                if (!string.IsNullOrEmpty(code))
                {
                    var expectedState = Session["spotify_oauth_state"] as string;
                    Session["spotify_oauth_state"] = null;
                    if (string.IsNullOrEmpty(expectedState) || !string.Equals(expectedState, state, StringComparison.Ordinal))
                    {
                        throw new InvalidOperationException("Invalid OAuth state.");
                    }

                    ExchangeCodeAndImportTracks(code);

                    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path), endResponse: true);
                    return;
                }

                BindGrid();
            }
        }

        protected void ImageButton1_Click1(object sender, EventArgs e)
        {
            var clientId = ConfigurationManager.AppSettings["SpotifyClientID"];
            
            var redirectUri = ConfigurationManager.AppSettings["SpotifyRedirectUri"];
            var scope = "user-library-read";
            var state = Guid.NewGuid().ToString("N");
            Session["spotify_oauth_state"] = state;

            var authorizeUrl = "https://accounts.spotify.com/authorize" +
                               "?response_type=code" +
                               "&client_id=" + HttpUtility.UrlEncode(clientId) +
                               "&scope=" + HttpUtility.UrlEncode(scope) +
                               "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
                               "&state=" + HttpUtility.UrlEncode(state);

            Response.Redirect(authorizeUrl, endResponse: true);
        }

        private void ExchangeCodeAndImportTracks(string code)
        {
            var clientId = ConfigurationManager.AppSettings["SpotifyClientID"];
            var clientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"];
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException("SpotifyClientID/SpotifyClientSecret must be configured in Web.config.");
            }

            var redirectUri = ConfigurationManager.AppSettings["SpotifyRedirectUri"];

            using (var http = new HttpClient())
            {
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

                var form = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("grant_type","authorization_code"),
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("redirect_uri", redirectUri)
                });

                var tokenResp = http.PostAsync(TokenEndpoint, form).Result;
                var tokenContent = tokenResp.Content.ReadAsStringAsync().Result;
                if (!tokenResp.IsSuccessStatusCode)
                    throw new InvalidOperationException("Token exchange failed: " + tokenContent);

                var tokenObj = JObject.Parse(tokenContent);
                var accessToken = (string)tokenObj["access_token"];

                ImportSavedTracksToDatabase(accessToken);
            }
        }

        private void ImportSavedTracksToDatabase(string accessToken)
        {
            var connStringSetting = ConfigurationManager.ConnectionStrings["OfflineStreamer"];
            if (connStringSetting == null || string.IsNullOrWhiteSpace(connStringSetting.ConnectionString))
                throw new InvalidOperationException("Connection string 'OfflineStreamer' is not configured in Web.config.");

            var connectionString = connStringSetting.ConnectionString;

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                int offset = 0;
                const int limit = 50;

                using (var sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();


                    var insertCmd = new SqlCommand("INSERT INTO Songs (SongName, ArtistName) VALUES (@song, @artist)", sqlConn);
                    insertCmd.Parameters.Add(new SqlParameter("@song", System.Data.SqlDbType.NVarChar));
                    insertCmd.Parameters.Add(new SqlParameter("@artist", System.Data.SqlDbType.NVarChar));

                    while (true)
                    {
                        var url = $"{SavedTracksEndpoint}?limit={limit}&offset={offset}";
                        var resp = http.GetAsync(url).Result;
                        var content = resp.Content.ReadAsStringAsync().Result;
                        if (!resp.IsSuccessStatusCode)
                            throw new InvalidOperationException("Failed to fetch saved tracks: " + content);

                        var obj = JObject.Parse(content);
                        var items = (JArray)obj["items"];
                        if (items == null || items.Count == 0)
                            break;

                        foreach (var item in items)
                        {
                            var track = item["track"];
                            if (track == null) continue;

                            var songName = (string)track["name"] ?? "";
                            var artistName = "";
                            var artists = track["artists"] as JArray;
                            if (artists != null && artists.Count > 0)
                                artistName = (string)artists[0]["name"] ?? "";

                            

                            insertCmd.Parameters["@song"].Value = songName;
                            insertCmd.Parameters["@artist"].Value = artistName; 
                            insertCmd.ExecuteNonQuery();
                            
                            
                        }

                        if (items.Count < limit)
                            break;

                        offset += limit;
                    }
                }
            }
        }

        

        private void BindGrid()
        {
            var connStringSetting = ConfigurationManager.ConnectionStrings["OfflineStreamer"];
            if (connStringSetting == null || string.IsNullOrWhiteSpace(connStringSetting.ConnectionString))
            {
                SongsGrid.DataSource = null;
                SongsGrid.DataBind();
                return;
            }

            var connectionString = connStringSetting.ConnectionString;
            var dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (var da = new SqlDataAdapter("SELECT ID, SongName, ArtistName FROM Songs ORDER BY ID DESC", conn))
            {
                da.Fill(dt);
            }

            SongsGrid.DataSource = dt;
            SongsGrid.DataBind();
        }

        protected void SongsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var key = SongsGrid.DataKeys[e.RowIndex];
            if (key == null) return;

            var idObj = key.Value;
            if (idObj == null) return;

            var connStringSetting = ConfigurationManager.ConnectionStrings["OfflineStreamer"];
            

            var connectionString = connStringSetting.ConnectionString;
            var id = Convert.ToInt32(idObj);

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Songs WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

        
        // output paths
        private const string YtDlpExePath = @"C:\Users\acerWin10\AppData\Local\Programs\Python\Python313\Scripts\yt-dlp.exe";
        private const string FfmpegBinPath = @"C:\Drivers\ffmpeg-8.0.1-essentials_build\bin";
        private const string DownloadOutputPath = @"D:\Music Streamer\Project\Offline_Streamer\App_Data\Songs";
        private const string LogPath = @"D:\temp\offline-streamer.log";

        // reads SongsGrid
        protected void Button1_Click1(object sender, EventArgs e)
        {
            var queries = new List<string>();

            foreach (GridViewRow row in SongsGrid.Rows)
            {
                var song = HttpUtility.HtmlDecode(row.Cells.Count > 1 ? row.Cells[1].Text : string.Empty).Trim();
                var artist = HttpUtility.HtmlDecode(row.Cells.Count > 2 ? row.Cells[2].Text : string.Empty).Trim();

                if (song == "&nbsp;") song = string.Empty;
                if (artist == "&nbsp;") artist = string.Empty;

                if (!string.IsNullOrWhiteSpace(song))
                {
                    queries.Add(string.IsNullOrWhiteSpace(artist) ? song : $"{song} - {artist}");
                }
            }

            if (queries.Count == 0)
            {
                Response.Write("<div style='color:#f8d7da;background:#3a0b09;padding:8px;border-radius:6px;margin:8px 0;'>No songs found in grid.</div>");
                return;
            }
            

             
                try
                {
                    DownloadVideosFromTitles(queries);
                    Response.Write("<div style='color:#d4edda;background:#083a12;padding:8px;border-radius:6px;margin:8px 0;'>Downloads completed.</div>");
                }
                catch (Exception ex)
                {
                    Log("ERROR: " + ex);
                    Response.Write("<div style='color:#f8d7da;background:#3a0b09;padding:8px;border-radius:6px;margin:8px 0;'>Download failed:</div>");
                }
            
            //Response.Redirect("~/Loading.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void DownloadVideosFromTitles(List<string> songQueries)
        {
            if (songQueries == null || songQueries.Count == 0) return;

            if (!File.Exists(YtDlpExePath))
                throw new InvalidOperationException("yt-dlp.exe not found at: " + YtDlpExePath);

            if (!Directory.Exists(FfmpegBinPath))
                throw new InvalidOperationException("ffmpeg bin folder not found at: " + FfmpegBinPath);

            Directory.CreateDirectory(DownloadOutputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath));

            var outputTemplate = Path.Combine(DownloadOutputPath, "%(title)s.%(ext)s");

            foreach (var query in songQueries)
            {
                var safeQuery = query.Replace("\"", "'");

                var args =
                    $"ytsearch1:\"{safeQuery}\" " +
                    "-x --audio-format mp3 --audio-quality 192K --no-playlist " +
                    $"--ffmpeg-location \"{FfmpegBinPath}\" " +
                    $"--no-overwrites " +
                    $"-o \"{outputTemplate}\"";

                var psi = new ProcessStartInfo
                {
                    FileName = YtDlpExePath,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                Log("RUN: " + psi.FileName + " " + psi.Arguments);

                using (var proc = Process.Start(psi))
                {
                    if (proc == null)
                        throw new InvalidOperationException("Failed to start yt-dlp process.");

                    var stdOutTask = proc.StandardOutput.ReadToEndAsync();
                    var stdErrTask = proc.StandardError.ReadToEndAsync();

                    proc.WaitForExit();

                    var stdOut = stdOutTask.Result;
                    var stdErr = stdErrTask.Result;

                    Log("EXIT: " + proc.ExitCode);
                    if (!string.IsNullOrWhiteSpace(stdOut)) Log("OUT: " + stdOut);
                    if (!string.IsNullOrWhiteSpace(stdErr)) Log("ERR: " + stdErr);

                    if (proc.ExitCode != 0)
                        throw new InvalidOperationException("yt-dlp failed for query '" + query + "'. " + stdErr);
                }
            }
        }

        private static void Log(string message)
        {
            File.AppendAllText(LogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }



    }
}
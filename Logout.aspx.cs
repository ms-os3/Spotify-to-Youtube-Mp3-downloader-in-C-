using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Offline_Streamer
{
    public partial class Streaming : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            // 1) Clear DB rows so GridView binds empty next time
            ClearSongsTable();

            // 2) Clear session/token/cache state
            Session.Clear();
            Session.RemoveAll();
            Session["spotify_oauth_state"] = null;
            Session["DownloadJobId"] = null; // if you used background job status
            HttpRuntime.Cache.Remove("spotify_token"); // if you cached tokens manually

            // 3) End auth/session cookies
            FormsAuthentication.SignOut();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var c = new HttpCookie("ASP.NET_SessionId", "")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }

            Session.Abandon();

            // 4) Back to OAuth start page (where ImageButton is)
            Response.Redirect("~/Default.aspx", true);
        }

        private void ClearSongsTable()
        {
            var cs = ConfigurationManager.ConnectionStrings["OfflineStreamer"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs)) return;

            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand("DELETE FROM Songs; DBCC CHECKIDENT ('Songs', RESEED, 0);", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
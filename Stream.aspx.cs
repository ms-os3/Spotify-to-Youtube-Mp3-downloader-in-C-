using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Offline_Streamer
{
    public partial class Stream : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var songsDir = Server.MapPath("~/App_Data/Songs");
            if (!Directory.Exists(songsDir))
            {
                EmptyPanel.Visible = true;
                SongsRepeater.DataSource = null;
                SongsRepeater.DataBind();
                return;
            }

            var tracks = new DirectoryInfo(songsDir)
                .GetFiles("*.mp3")
                .OrderBy(f => f.Name)
                .Select(f => new
                {
                    Title = Path.GetFileNameWithoutExtension(f.Name),
                    Url = ResolveUrl("~/Audio.ashx?f=" + Uri.EscapeDataString(f.Name))
                })
                .ToList();

            EmptyPanel.Visible = tracks.Count == 0;
            SongsRepeater.DataSource = tracks;
            SongsRepeater.DataBind();
        }
    }
}

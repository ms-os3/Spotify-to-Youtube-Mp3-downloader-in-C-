using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Offline_Streamer
{
    public partial class Loading : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var jobId = Session["DownloadJobId"] as string;
            if (string.IsNullOrWhiteSpace(jobId))
            {
                Response.Redirect("~/Downloads.aspx");
                return;
            }

            var status = DownloadJobStore.Get(jobId);

            if (status == "done")
            {
                Response.Redirect("~/Stream.aspx");
                return;
            }

            if (status.StartsWith("error:"))
            {
                StatusLabel.Text = status; // add a Label on Loading.aspx
                return;
            }

            StatusLabel.Text = "...";
        }
    }
}
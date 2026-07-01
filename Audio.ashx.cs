using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Offline_Streamer
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var fileName = Path.GetFileName(context.Request.QueryString["f"] ?? "");
            if (string.IsNullOrWhiteSpace(fileName))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var path = context.Server.MapPath("~/App_Data/Songs/" + fileName);
            if (!File.Exists(path))
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "audio/mpeg";
            context.Response.AddHeader("Accept-Ranges", "bytes");
            context.Response.TransmitFile(path);
        
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
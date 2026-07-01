using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Offline_Streamer
{
    public static class DownloadJobStore
    {
        private static readonly ConcurrentDictionary<string, string> _jobs = new ConcurrentDictionary<string, string>();
        public static void Set(string id, string status) => _jobs[id] = status;
        public static string Get(string id) => _jobs.TryGetValue(id, out var s) ? s : "unknown";
    }
}
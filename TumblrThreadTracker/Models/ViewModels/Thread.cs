using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.ViewModels
{
    public class Thread
    {
        public int UserThreadId { get; set; }
        public long PostId { get; set; }
        public string UserTitle { get; set; }
        public string Type { get; set; }
        public string ContentSnippet { get; set; }
        public string BlogShortname { get; set; }
        public string LastPosterShortname { get; set; }
        public string LastPostUrl { get; set; }
        public bool IsMyTurn { get; set; }
    }
}
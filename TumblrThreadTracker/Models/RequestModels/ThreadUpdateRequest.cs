﻿using System.Collections.Generic;

namespace TumblrThreadTracker.Models.RequestModels
{
    public class ThreadUpdateRequest
    {
        public int? UserThreadId { get; set; }
        public string PostId { get; set; }
        public string BlogShortname { get; set; }
        public string UserTitle { get; set; }
        public string WatchedShortname { get; set; }
        public List<string> ThreadTags { get; set; }
        public bool IsArchived { get; set; }
    }
}
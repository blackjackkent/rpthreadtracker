using System.Collections.Generic;
using NUnit.Framework;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Models.DomainModels.Threads
{
    public class ThreadDto : IDto<Thread>
    {
        public int? UserThreadId { get; set; }
        public string PostId { get; set; }
        public string UserTitle { get; set; }
        public string Type { get; set; }
        public string ContentSnippet { get; set; }
        public int UserBlogId { get; set; }
        public string BlogShortname { get; set; }
        public string LastPosterShortname { get; set; }
        public string WatchedShortname { get; set; }
        public string LastPostUrl { get; set; }
        public long? LastPostDate { get; set; }
        public bool IsMyTurn { get; set; }
        public bool IsArchived { get; set; }
        public List<string> ThreadTags { get; set; }

        public Thread ToModel()
        {
            return new Thread(this);
        }
    }
}
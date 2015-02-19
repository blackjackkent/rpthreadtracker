namespace TumblrThreadTracker.Models.DomainModels.Threads
{
    public class ThreadDto
    {
        public int? UserThreadId { get; set; }
        public long PostId { get; set; }
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

        public Thread ToModel()
        {
            return new Thread(this);
        }
    }
}
namespace TumblrThreadTracker.Models.RequestModels
{
    public class BlogUpdateRequest
    {
        public int? UserBlogId { get; set; }
        public string BlogShortname { get; set; }
    }
}
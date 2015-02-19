namespace TumblrThreadTracker.Models.DomainModels.Blogs
{
    public class BlogDto
    {
        public int? UserBlogId { get; set; }
        public int UserId { get; set; }
        public string BlogShortname { get; set; }

        public Blog ToModel()
        {
            return new Blog(this);
        }
    }
}
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTrackerTests.TestBuilders
{
    public class BlogBuilder : Builder<Blog, BlogDto>
    {
        public BlogBuilder()
            : base(GetDefaultValues())
        {
        }

        private static BlogDto GetDefaultValues()
        {
            return new BlogDto()
            {
                BlogShortname = "cmdr-blackjack-shepard",
                UserBlogId = 15,
                UserId = 10
            };
        }

        public BlogBuilder WithBlogShortname(string blogShortname)
        {
            Dto.BlogShortname = blogShortname;
            return this;
        }

        public BlogBuilder WithUserBlogId(int? userBlogId)
        {
            Dto.UserBlogId = userBlogId;
            return this;
        }

        public BlogBuilder WithUserId(int userId)
        {
            Dto.UserId = userId;
            return this;
        }
    }
}
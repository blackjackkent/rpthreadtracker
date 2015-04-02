using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTrackerTests.TestBuilders.Domain
{
    public class ThreadBuilder : DomainBuilder<Thread, ThreadDto>
    {
        public ThreadBuilder()
            : base(GetDefaultValues())
        {
        }

        private static ThreadDto GetDefaultValues()
        {
            return new ThreadDto()
            {
                BlogShortname = "cmdr-blackjack-shepard",
                ContentSnippet = null,
                IsMyTurn = true,
                LastPostDate = 1234567,
                LastPostUrl = "http://alpha-of-omega.tumblr.com/post/12345",
                LastPosterShortname = "alpha-of-omega",
                PostId = 12345,
                Type = "post",
                UserBlogId = 15,
                UserThreadId = 20,
                UserTitle = "Test Thread",
                WatchedShortname = "alpha-of-omega"
            };
        }

        public ThreadBuilder WithBlogShortname(string blogShortname) {
            Dto.BlogShortname = blogShortname;
            return this;
        }
        public ThreadBuilder WithContentSnippet(string contentSnippet) {
            Dto.ContentSnippet = contentSnippet;
            return this;
        }
        public ThreadBuilder WithIsMyTurn(bool isMyTurn) {
            Dto.IsMyTurn = isMyTurn;
            return this;
        }
        public ThreadBuilder WithLastPostDate(long? lastPostDate) {
            Dto.LastPostDate = lastPostDate;
            return this;
        }
        public ThreadBuilder WithLastPostUrl(string lastPostUrl) {
            Dto.LastPostUrl = lastPostUrl;
            return this;
        }
        public ThreadBuilder WithLastPosterShortname(string lastPosterShortname) {
            Dto.LastPosterShortname = lastPosterShortname;
            return this;
        }
        public ThreadBuilder WithPostId(int postId) {
            Dto.PostId = postId;
            return this;
        }
        public ThreadBuilder WithType(string type) {
            Dto.Type = type;
            return this;
        }
        public ThreadBuilder WithUserBlogId(int userBlogId) {
            Dto.UserBlogId = userBlogId;
            return this;
        }
        public ThreadBuilder WithUserThreadId(int userThreadId) {
            Dto.UserThreadId = userThreadId;
            return this;
        }
        public ThreadBuilder WithUserTitle(string userTitle) {
            Dto.UserTitle = userTitle;
            return this;
        }
        public ThreadBuilder WithWatchedShortname(string watchedShortname) {
            Dto.WatchedShortname = watchedShortname;
            return this;
        }
    }
}

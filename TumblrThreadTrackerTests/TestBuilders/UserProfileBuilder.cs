using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTrackerTests.TestBuilders
{
    public class UserProfileBuilder : Builder<UserProfile, UserProfileDto>
    {
        public UserProfileBuilder()
            : base(GetDefaultValues())
        {
        }

        private static UserProfileDto GetDefaultValues()
        {
            return new UserProfileDto()
            {
                UserId = 10,
                UserName = "blackjackkent",
                Email = "test@test.com"
            };
        }

        public UserProfileBuilder WithUserName(string username)
        {
            Dto.UserName = username;
            return this;
        }

        public UserProfileBuilder WithEmail(string email)
        {
            Dto.Email = email;
            return this;
        }

        public UserProfileBuilder WithUserId(int userId)
        {
            Dto.UserId = userId;
            return this;
        }
    }
}
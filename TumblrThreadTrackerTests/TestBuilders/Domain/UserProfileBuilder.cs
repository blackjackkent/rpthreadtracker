using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTrackerTests.TestBuilders.Domain
{
    public class UserProfileBuilder : DomainBuilder<User, UserDto>
    {
        public UserProfileBuilder()
            : base(GetDefaultValues())
        {
        }

        private static UserDto GetDefaultValues()
        {
            return new UserDto()
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
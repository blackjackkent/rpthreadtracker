using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Models.DomainModels.Users
{
    public class UserProfileDto : IDto<UserProfile>
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserProfile ToModel()
        {
            return new UserProfile(this);
        }
    }
}
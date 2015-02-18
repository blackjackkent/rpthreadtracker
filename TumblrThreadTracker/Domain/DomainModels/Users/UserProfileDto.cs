namespace TumblrThreadTracker.Domain.Users
{
    public class UserProfileDto
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
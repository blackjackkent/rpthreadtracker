using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserProfileService
    {
        UserProfileDto GetByUserId(int id, IRepository<UserProfile> userProfileRepository);
        UserProfileDto GetByUsername(string username, IRepository<UserProfile> userProfileRepository);
    }
}
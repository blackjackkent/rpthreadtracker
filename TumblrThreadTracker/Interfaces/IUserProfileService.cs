using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserProfileService
    {
        void Update(UserDto user, IRepository<User> userProfileRepository);
    }
}
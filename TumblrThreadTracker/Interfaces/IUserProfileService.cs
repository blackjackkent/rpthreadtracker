using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Interfaces
{
    public interface IUserProfileService
    {
        UserDto GetByUserId(int id, IRepository<User> userProfileRepository);
        UserDto GetByUsername(string username, IRepository<User> userProfileRepository);
        void Update(UserDto user, IRepository<User> userProfileRepository);
    }
}
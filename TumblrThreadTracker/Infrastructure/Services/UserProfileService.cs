using System.Linq;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        public void Update(UserDto user, IRepository<User> userProfileRepository)
        {
            userProfileRepository.Update(user.UserId, user.ToModel());
        }
    }
}
using System.Linq;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        public UserDto GetByUserId(int id, IRepository<User> userProfileRepository)
        {
            var profile = userProfileRepository.GetSingle(p => p.UserId == id);
            return profile == null ? null : profile.ToDto();
        }

        public UserDto GetByUsername(string username, IRepository<User> userProfileRepository)
        {
            var profile = userProfileRepository.Get(u => u.UserName == username).FirstOrDefault();
            return profile == null ? null : profile.ToDto();
        }

        public void Update(UserDto user, IRepository<User> userProfileRepository)
        {
            userProfileRepository.Update(user.UserId, user.ToModel());
        }
    }
}
using System.Linq;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        public UserProfileDto GetByUserId(int id, IRepository<UserProfile> userProfileRepository)
        {
            var profile = userProfileRepository.Get(id);
            return profile == null ? null : profile.ToDto();
        }

        public UserProfileDto GetByUsername(string username, IRepository<UserProfile> userProfileRepository)
        {
            var profile = userProfileRepository.Get(u => u.UserName == username).FirstOrDefault();
            return profile == null ? null : profile.ToDto();
        }
    }
}
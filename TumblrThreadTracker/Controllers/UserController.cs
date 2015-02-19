using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UserController(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public UserProfileDto Get()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            return _userProfileRepository.Get(userId).ToDto();
        }
    }
}
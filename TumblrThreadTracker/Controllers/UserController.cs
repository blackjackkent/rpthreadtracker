using System.Web.Http;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Repositories;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserController()
        {
            _userProfileRepository = new UserProfileRepository(new ThreadTrackerContext());
        }

        public UserProfileDto Get()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            return _userProfileRepository.GetUserProfileById(userId).ToDto();
        }
    }
}
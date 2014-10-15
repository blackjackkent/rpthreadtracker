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
        private static int _userId;
        private readonly IUserProfileRepository _userProfileRepository;

        public UserController()
        {
            _userProfileRepository = new UserProfileRepository(new ThreadTrackerContext());
            _userId = WebSecurity.GetUserId(User.Identity.Name);
        }

        public UserProfileDto Get()
        {
            return _userProfileRepository.GetUserProfileById(_userId).ToDto();
        }
    }
}
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IRepository<User> _userProfileRepository;
        private readonly IWebSecurityService _webSecurityService;

        public UserController(IRepository<User> userProfileRepository, IWebSecurityService webSecurityService)
        {
            _userProfileRepository = userProfileRepository;
            _webSecurityService = webSecurityService;
        }

        public UserDto Get()
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            return _userProfileRepository.GetSingle(u => u.UserId == userId).ToDto();
        }
    }
}
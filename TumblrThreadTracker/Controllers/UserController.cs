using System.Security.Claims;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IWebSecurityService _webSecurityService;

        public UserController(IWebSecurityService webSecurityService)
        {
            _webSecurityService = webSecurityService;
        }

        public UserDto Get()
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            return user.ToDto();
        }
    }
}
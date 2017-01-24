using System;
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
        private readonly IUserProfileService _userProfileService;
        private readonly IRepository<User> _userProfileRepository;

        public UserController(IWebSecurityService webSecurityService, IUserProfileService userProfileService, IRepository<User> userProfileRepository)
        {
            _webSecurityService = webSecurityService;
            _userProfileService = userProfileService;
            _userProfileRepository = userProfileRepository;
        }

        public UserDto Get()
        {
			var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            return user.ToDto();
        }

        public void Put(UserDto user)
        {
            var currentUser = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity) User.Identity);
            if (currentUser.UserId != user.UserId)
            {
                throw new ArgumentException();
            }
            _userProfileService.Update(user, _userProfileRepository);
        }
    }
}
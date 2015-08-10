using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    public class SessionController : ApiController
    {
        private readonly IWebSecurityService _webSecurityService;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public SessionController(IWebSecurityService webSecurityService, IRepository<UserProfile> userProfileRepository)
        {
            _webSecurityService = webSecurityService;
            _userProfileRepository = userProfileRepository;
        }
        [HttpPost]
        public HttpResponseMessage Post(LoginRequest model)
        {
            if (model == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            var isLoggedIn = _webSecurityService.Login(model.UserName, model.Password);
            if (isLoggedIn)
            {
                var account = _userProfileRepository.Get(_webSecurityService.GetUserId(model.UserName));
                account.SetLastLogin(DateTime.Now, _userProfileRepository);
            }
            return isLoggedIn 
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [HttpDelete]
        public void Delete()
        {
            _webSecurityService.Logout();
        }
    }
}
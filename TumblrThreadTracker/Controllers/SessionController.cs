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
        private readonly IRepository<User> _userProfileRepository;

        public SessionController(IWebSecurityService webSecurityService, IRepository<User> userProfileRepository)
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
                var userId = _webSecurityService.GetUserId(model.UserName);
                var account = _userProfileRepository.GetSingle(u => u.UserId == userId);
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
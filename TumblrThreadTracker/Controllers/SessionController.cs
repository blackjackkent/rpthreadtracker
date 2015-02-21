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

        public SessionController(IWebSecurityService webSecurityService)
        {
            _webSecurityService = webSecurityService;
        }
        [HttpPost]
        public HttpResponseMessage Post(LoginRequest model)
        {
            if (model == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            return _webSecurityService.Login(model.UserName, model.Password, rememberMe: model.RememberMe)
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
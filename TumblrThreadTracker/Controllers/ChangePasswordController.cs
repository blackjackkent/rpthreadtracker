using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class ChangePasswordController : ApiController
    {
        private readonly IWebSecurityService _webSecurityService;

        public ChangePasswordController(IWebSecurityService webSecurityService)
        {
            _webSecurityService = webSecurityService;
        }
        [HttpPost]
        public HttpResponseMessage ChangePassword(ChangePasswordRequest model)
        {
            _webSecurityService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
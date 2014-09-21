using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class ChangePasswordController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ChangePassword(ChangePasswordRequest model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            if (!hasLocalAccount)
                throw new InvalidOperationException();
            WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
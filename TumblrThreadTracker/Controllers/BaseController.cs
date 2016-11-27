using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;

namespace TumblrThreadTracker.Controllers
{
    public class BaseController : ApiController
    {
        protected ClaimsIdentity UserIdentity;
        protected IWebSecurityService WebSecurityService;

        public BaseController(IWebSecurityService webSecurityService)
        {
            WebSecurityService = webSecurityService;
            UserIdentity = (ClaimsIdentity)User.Identity;
        }
    }
}
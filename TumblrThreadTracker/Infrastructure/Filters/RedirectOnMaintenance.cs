using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TumblrThreadTracker.Infrastructure.Filters
{
    public class RedirectOnMaintenance : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            var allowedIPs = ConfigurationManager.AppSettings["AllowedMaintenanceIPs"].Split(',');

            if (ConfigurationManager.AppSettings["MaintenanceMode"] == "true" && !allowedIPs.Contains(ipAddress))
            {
                filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Temporarily Offline");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
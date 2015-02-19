using System.Web.Mvc;
using System.Web.Routing;

namespace TumblrThreadTracker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("DefaultRoute", "{*url}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional});
        }
    }
}
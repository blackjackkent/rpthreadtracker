namespace TumblrThreadTracker
{
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Class defining setup of MVC routes
	/// </summary>
	public class RouteConfig
	{
		/// <summary>
		/// Initialize routes for MVC views
		/// </summary>
		/// <remarks>
		/// This initializes the MVC view which is used to hold all
		/// front-end angular logic. It also ensures the routing
		/// ignores axd files (including Elmah) and the Owin token handler.
		/// </remarks>
		/// <param name="routes">Default route collection object</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("token");
			routes.MapRoute("DefaultRoute", "{*url}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}
	}
}
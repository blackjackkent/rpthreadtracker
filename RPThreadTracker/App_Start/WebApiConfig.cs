namespace RPThreadTracker
{
	using System.Configuration;
	using System.Web.Http;
	using System.Web.Http.ExceptionHandling;
	using System.Web.Optimization;
	using System.Web.Routing;
	using Elmah.Contrib.WebApi;
	using WebMatrix.WebData;

	/// <summary>
	/// Class which initializes all WebAPI settings across the project
	/// </summary>
	public static class WebApiConfig
	{
		/// <summary>
		/// Initialize settings for WebAPI project
		/// </summary>
		/// <param name="config">Default HttpConfiguration object</param>
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			WebSecurity.InitializeDatabaseConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, "System.Data.SqlClient", "UserProfile", "UserId", "UserName", true);
			AutoMapperConfiguration.Configure();
			config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
		}
	}
}
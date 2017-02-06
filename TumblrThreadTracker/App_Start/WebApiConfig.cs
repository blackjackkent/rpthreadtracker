namespace TumblrThreadTracker
{
	using System.Web.Http;
	using System.Web.Http.ExceptionHandling;
	using System.Web.Optimization;
	using System.Web.Routing;
	using Elmah.Contrib.WebApi;
	using Infrastructure;
	using Infrastructure.Repositories;
	using Infrastructure.Services;
	using Interfaces;
	using Microsoft.Practices.Unity;
	using Models.DomainModels.Account;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using Models.DomainModels.Users;
	using RestSharp;
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
			WebSecurity.InitializeDatabaseConnection(
				"DefaultConnection",
				"UserProfile",
				"UserId",
				"UserName",
				true);
			AutoMapperConfiguration.Configure();
			config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
		}
	}
}
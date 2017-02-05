using Microsoft.Owin;
[assembly: OwinStartup(typeof(TumblrThreadTracker.Startup))]
namespace TumblrThreadTracker
{
	using System;
	using System.Web.Http;
	using Infrastructure.Providers;
	using Microsoft.Owin.Security.OAuth;
	using Owin;

	/// <summary>
	/// Startup class for initializing OAuth configuration
	/// and other WebAPI setup processes
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Initializes all WebAPI setup processes
		/// </summary>
		/// <param name="app">Default <see cref="IAppBuilder"/> WebAPI object</param>
		public void Configuration(IAppBuilder app)
		{
			ConfigureOAuth(app);
			var config = new HttpConfiguration();
			WebApiConfig.Register(config);
			app.UseWebApi(config);
		}

		private static void ConfigureOAuth(IAppBuilder app)
		{
			// Token Generation
			app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
				Provider = new SimpleAuthorizationServerProvider()
			});
			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}
	}
}
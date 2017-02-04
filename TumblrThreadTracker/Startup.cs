using Microsoft.Owin;

[assembly: OwinStartup(typeof(TumblrThreadTracker.Startup))]

namespace TumblrThreadTracker
{
	using System;
	using System.Web.Http;

	using Microsoft.Owin.Security.OAuth;

	using Owin;

	using TumblrThreadTracker.Infrastructure.Providers;

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureOAuth(app);
			HttpConfiguration config = new HttpConfiguration();
			WebApiConfig.Register(config);
			app.UseWebApi(config);
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
				                                                     {
					                                                     AllowInsecureHttp = true,
					                                                     TokenEndpointPath =
						                                                     new PathString("/token"),
					                                                     AccessTokenExpireTimeSpan
						                                                     = TimeSpan.FromDays(14),
					                                                     Provider =
						                                                     new SimpleAuthorizationServerProvider
							                                                     ()
				                                                     };

			// Token Generation
			app.UseOAuthAuthorizationServer(OAuthServerOptions);
			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}
	}
}
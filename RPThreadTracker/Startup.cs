using Microsoft.Owin;
using RPThreadTracker.Infrastructure.Filters;

[assembly: OwinStartup(typeof(RPThreadTracker.Startup))]
namespace RPThreadTracker
{
	using System;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Filters;
	using Infrastructure;
	using Infrastructure.Data;
	using Infrastructure.Providers;
	using Infrastructure.Repositories;
	using Infrastructure.Services;
	using Interfaces;
	using Microsoft.Owin.Security.OAuth;
	using Microsoft.Practices.Unity;
	using Models.DomainModels.Account;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using Models.DomainModels.Users;
	using Owin;
	using RestSharp;

	/// <summary>
	/// Startup class for initializing OAuth configuration
	/// and other WebAPI setup processes
	/// </summary>
	[ExcludeFromCoverage]
	public class Startup
	{
		/// <summary>
		/// Initializes all WebAPI setup processes
		/// </summary>
		/// <param name="app">Default <c>IAppBuilder</c> WebAPI object</param>
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			var container = ConfigureInjection(config);
			ConfigureOAuth(app, container);
			ConfigureFilters(config, container);
			WebApiConfig.Register(config);
			app.UseWebApi(config);
		}

		private static UnityContainer ConfigureInjection(HttpConfiguration config)
		{
			var container = new UnityContainer();
			container.RegisterType<IRepository<User>, UserProfileRepository>()
				.RegisterType<IRepository<Blog>, UserBlogRepository>()
				.RegisterType<IRepository<Thread>, UserThreadRepository>()
				.RegisterType<IRepository<Membership>, MembershipRepository>()
				.RegisterType<IWebSecurityService, WebSecurityService>()
				.RegisterType<IBlogService, BlogService>()
				.RegisterType<IThreadService, ThreadService>()
				.RegisterType<IUserProfileService, UserProfileService>()
				.RegisterType<IConfigurationService, ConfigurationService>()
				.RegisterType<ITumblrClient, TumblrClient>(new InjectionConstructor(new RestClient("http://api.tumblr.com/v2"), new ResolvedParameter<IConfigurationService>()))
				.RegisterType<IEmailClient, SendGridEmailClient>()
				.RegisterType<IExporterService, ExporterService>()
				.RegisterType<IThreadTrackerContext, RPThreadTrackerEntities>();
			config.DependencyResolver = new UnityResolver(container);
			return container;
		}

		private static void ConfigureOAuth(IAppBuilder app, UnityContainer container)
		{
			// Token Generation
			var userProfileRepository = container.Resolve<IRepository<User>>();
			var webSecurityService = container.Resolve<IWebSecurityService>();
			app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
				Provider = new SimpleAuthorizationServerProvider(webSecurityService, userProfileRepository)
			});
			app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}

		private void ConfigureFilters(HttpConfiguration config, UnityContainer container)
		{
			var providers = config.Services.GetFilterProviders().ToList();
			var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
			config.Services.Remove(typeof(IFilterProvider), defaultprovider);
			config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(container));
		}
	}
}
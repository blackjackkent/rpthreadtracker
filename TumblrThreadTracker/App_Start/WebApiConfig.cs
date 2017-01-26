using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using RestSharp;
using TumblrThreadTracker.Infrastructure;
using TumblrThreadTracker.Infrastructure.Repositories;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.DomainModels.Users;
using WebMatrix.WebData;

namespace TumblrThreadTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			var container = new UnityContainer();
            container.RegisterType<IRepository<User>, UserProfileRepository>()
                .RegisterType<IRepository<Blog>, UserBlogRepository>()
                .RegisterType<IRepository<Thread>, UserThreadRepository>()
                .RegisterType<IRepository<WebpagesMembership>, WebpagesMembershipRepository>()
                .RegisterType<IWebSecurityService, WebSecurityService>()
                .RegisterType<IBlogService, BlogService>()
                .RegisterType<IThreadService, ThreadService>()
                .RegisterType<IUserProfileService, UserProfileService>()
                .RegisterType<ITumblrClient, TumblrClient>(new InjectionConstructor(new RestClient("http://api.tumblr.com/v2")))
                .RegisterType<IEmailService, EmailService>()
				.RegisterType<IExporterService, ExporterService>()
				.RegisterType<IThreadTrackerContext, RPThreadTrackerEntities>();
            config.DependencyResolver = new UnityResolver(container);

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            AutoMapperConfiguration.Configure();
        }
    }
}
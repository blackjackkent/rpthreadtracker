using System.Web.Http;
using Microsoft.Practices.Unity;
using TumblrThreadTracker.Infrastructure;
using TumblrThreadTracker.Infrastructure.Repositories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IRepository<UserProfile>, UserProfileRepository>()
                .RegisterType<IRepository<Blog>, UserBlogRepository>()
                .RegisterType<IRepository<Thread>, UserThreadRepository>()
                .RegisterType<IRepository<WebpagesMembership>, WebpagesMembershipRepository>()
                .RegisterType<IThreadTrackerContext, ThreadTrackerContext>();
            config.DependencyResolver = new UnityResolver(container);

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }
    }
}
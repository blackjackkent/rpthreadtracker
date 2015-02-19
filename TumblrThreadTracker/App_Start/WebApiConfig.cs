using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Domain.Users;
using TumblrThreadTracker.Infrastructure.Repositories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;
using TumblrThreadTracker.Repositories;

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
                     .RegisterType<IRepository<webpages_Membership>, webpages_MembershipRepository>()
                     .RegisterType<IThreadTrackerContext, ThreadTrackerContext>();
            config.DependencyResolver = new UnityResolver(container);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

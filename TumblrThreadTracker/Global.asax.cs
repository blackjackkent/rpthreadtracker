using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace TumblrThreadTracker
{
    using Domain.Blogs;
    using Domain.Threads;
    using Domain.Users;
    using Infrastructure;
    using Infrastructure.Repositories;
    using Interfaces;
    using Microsoft.Practices.Unity;
    using Models;
    using Models.DataModels;
    using Repositories;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var container = new UnityContainer();
            container.RegisterType<IRepository<UserProfile>, UserProfileRepository>()
                     .RegisterType<IRepository<Blog>, UserBlogRepository>()
                     .RegisterType<IRepository<Thread>, UserThreadRepository>()
                     .RegisterType<IRepository<webpages_Membership>, webpages_MembershipRepository>()
                     .RegisterType<IThreadTrackerContext, ThreadTrackerContext>();
            DependencyResolver.SetResolver(new UnityResolver(container));
        }
    }
}
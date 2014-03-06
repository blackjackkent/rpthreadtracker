using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TumblrThreadTracker.Models;
using DataModels = TumblrThreadTracker.Models.DataModels;
using ViewModels = TumblrThreadTracker.Models.ViewModels;
using TumblrThreadTracker.Services;
using WebMatrix.WebData;
using TumblrThreadTracker.Factories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Repositories;

namespace TumblrThreadTracker.Controllers
{
    public class HomeController : Controller
    {
        private IUserBlogRepository _blogRepository;
        private IUserThreadRepository _threadRepository;

        public HomeController()
        {
            _blogRepository = new UserBlogRepository(new DataModels.ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new DataModels.ThreadTrackerContext());
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Threads()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            IEnumerable<ViewModels.UserBlog> blogs = BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(WebSecurity.GetUserId(User.Identity.Name)));
            foreach (ViewModels.UserBlog blog in blogs)
            {
                IEnumerable<DataModels.UserThread> dataThreads = _threadRepository.GetUserThreads(blog.UserBlogId);
                foreach (DataModels.UserThread dataThread in dataThreads)
                {
                    ViewModels.Thread viewThread = ThreadService.GetThread(dataThread.PostId, blog.BlogShortname, dataThread.UserTitle);
                    blog.Threads.Add(viewThread);
                }
            }
            ViewModels.ThreadManager manager = new ViewModels.ThreadManager
            {
                UserId = WebSecurity.GetUserId(User.Identity.Name),
                UserBlogs = blogs
            };
            return View(manager);
        }

        [HttpPost]
        public ActionResult ConnectBlog(ViewModels.UserBlog viewBlog)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            DataModels.UserBlog blog = BlogFactory.BuildFromViewModel(viewBlog, WebSecurity.GetUserId(User.Identity.Name));
            _blogRepository.InsertUserBlog(blog);
            return RedirectToAction("Threads");
        }

        public ActionResult RemoveBlog(int userBlogId)
        {
            _blogRepository.DeleteUserBlog(userBlogId);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult TrackThread(string postId, int userBlogId, string userTitle)
        {
            DataModels.UserThread thread = ThreadFactory.BuildDataModel(postId, userBlogId, userTitle);
            _threadRepository.InsertUserThread(thread);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

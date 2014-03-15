using System.Collections.Generic;
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
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
        }

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Threads");
        }

        public ActionResult Threads()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult GetThreads()
        {
            IEnumerable<ViewModels.UserBlog> blogs = BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(WebSecurity.GetUserId(User.Identity.Name)));
            var manager = new ViewModels.ThreadManager
            {
                UserId = WebSecurity.GetUserId(User.Identity.Name),
                UserBlogs = blogs
            };
            var viewThreads = new List<ViewModels.Thread>();
            foreach (var blog in blogs)
            {
                var dataThreads = _threadRepository.GetUserThreads(blog.UserBlogId);
                foreach (var dataThread in dataThreads)
                {
                    ViewModels.Thread viewThread = ThreadService.GetThread(dataThread.PostId, blog.BlogShortname, dataThread.UserTitle);
                    if (viewThread != null)
                    {
                        viewThreads.Add(viewThread);
                    }
                }
            }
            manager.Threads = viewThreads;
            var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jsonSerializer.Serialize(manager);
            return Content(json, "application/json");
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

        [HttpPost]
        public ActionResult TrackThread(string postId, int userBlogId, string userTitle)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            DataModels.UserThread thread = ThreadFactory.BuildDataModel(postId, userBlogId, userTitle);
            _threadRepository.InsertUserThread(thread);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult UntrackThread(string postId)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            _threadRepository.DeleteUserThreadByPostId(postId);
            return RedirectToAction("Threads", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

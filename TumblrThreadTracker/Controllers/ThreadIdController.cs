using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TumblrThreadTracker.Factories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;
using TumblrThreadTracker.Repositories;
using UserBlog = TumblrThreadTracker.Models.ViewModels.UserBlog;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class ThreadIdController : ApiController
    {
        private readonly IUserBlogRepository _blogRepository;
        private readonly IUserThreadRepository _threadRepository;

        public ThreadIdController()
        {
            _blogRepository = new UserBlogRepository(new ThreadTrackerContext());
            _threadRepository = new UserThreadRepository(new ThreadTrackerContext());
        }

        public IEnumerable<int> Get(int id)
        {
            IEnumerable<UserBlog> blogs = BlogFactory.BuildFromDataModel(_blogRepository.GetUserBlogs(id));
            var threadIds = new List<int>();
            foreach (UserBlog blog in blogs)
            {
                IEnumerable<UserThread> dataThreads = _threadRepository.GetUserThreads(blog.UserBlogId);
                threadIds.AddRange(dataThreads.Select(t => t.UserThreadId));
            }
            return threadIds;
        }

        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
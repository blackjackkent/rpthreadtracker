using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [Authorize]
    public class ThreadController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;

        public ThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
        }

        public ThreadDto Get(int id)
        {
            return Thread.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int?> Get()
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var ids = new List<int?>();
            var blogs = Blog.GetBlogsByUserId(userId, _blogRepository);
            foreach (var blog in blogs)
                ids.AddRange(Thread.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            return ids;
        }

        public void Post(ThreadUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException();
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var blog = Blog.GetBlogByShortname(request.BlogShortname, userId, _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = null,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname
            };
            Thread.AddNewThread(dto, _threadRepository);
        }

        public void Put(ThreadUpdateRequest request)
        {
            if (request == null || request.UserThreadId == null)
                throw new ArgumentNullException();
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var blog = Blog.GetBlogByShortname(request.BlogShortname, userId, _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = request.UserThreadId,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname
            };
            Thread.UpdateThread(dto, _threadRepository);
        }

        public void Delete(int userThreadId)
        {
            var userId = WebSecurity.GetUserId(User.Identity.Name);
            var thread = Thread.GetById(userThreadId, _blogRepository, _threadRepository);
            var blog = Blog.GetBlogById(thread.UserBlogId, _blogRepository);
            if (blog.UserId != userId)
                return;
            Thread.DeleteThread(userThreadId, _threadRepository);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Services;
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
        private readonly IWebSecurityService _webSecurityService;
        private readonly IBlogService _blogService;
        private readonly IThreadService _threadService;

        public ThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
            _threadService = threadService;
        }

        public ThreadDto Get(int id)
        {
            return _threadService.GetById(id, _blogRepository, _threadRepository);
        }

        public IEnumerable<int?> Get()
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var ids = new List<int?>();
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository);
            foreach (var blog in blogs)
                ids.AddRange(_threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
            return ids;
        }

        public void Post(ThreadUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException();
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var blog = _blogService.GetBlogByShortname(request.BlogShortname, userId, _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = null,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname
            };
            _threadService.AddNewThread(dto, _threadRepository);
        }

        public void Put(ThreadUpdateRequest request)
        {
            if (request == null || request.UserThreadId == null)
                throw new ArgumentNullException();
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var blog = _blogService.GetBlogByShortname(request.BlogShortname, userId, _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = request.UserThreadId,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname
            };
            _threadService.UpdateThread(dto, _threadRepository);
        }

        public void Delete(int userThreadId)
        {
            var userId = _webSecurityService.GetUserId(User.Identity.Name);
            var thread = _threadService.GetById(userThreadId, _blogRepository, _threadRepository);
            var blog = _blogService.GetBlogById(thread.UserBlogId, _blogRepository);
            if (blog.UserId != userId)
                return;
            _threadService.DeleteThread(userThreadId, _threadRepository);
        }
    }
}
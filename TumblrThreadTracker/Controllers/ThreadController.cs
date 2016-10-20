using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.RequestModels;
using WebMatrix.WebData;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    [Authorize]
    public class ThreadController : ApiController
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Thread> _threadRepository;
        private readonly IWebSecurityService _webSecurityService;
        private readonly IBlogService _blogService;
        private readonly IThreadService _threadService;
        private readonly ITumblrClient _tumblrClient;

        public ThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService, ITumblrClient tumblrClient)
        {
            _blogRepository = userBlogRepository;
            _threadRepository = userThreadRepository;
            _webSecurityService = webSecurityService;
            _blogService = blogService;
            _threadService = threadService;
            _tumblrClient = tumblrClient;
        }

        public ThreadDto Get(int id)
        {
            return _threadService.GetById(id, _blogRepository, _threadRepository, _tumblrClient);
        }

        public IEnumerable<int?> Get([FromUri] bool isArchived = false)
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
            var ids = new List<int?>();
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository);
            foreach (var blog in blogs)
                ids.AddRange(_threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository, isArchived));
            return ids;
        }

        public void Post(ThreadUpdateRequest request)
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
            if (request == null || userId == null)
                throw new ArgumentNullException();
            var blog = _blogService.GetBlogByShortname(request.BlogShortname, userId.GetValueOrDefault(), _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = null,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId ?? -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname,
                ThreadTags = request.ThreadTags
            };
            _threadService.AddNewThread(dto, _threadRepository);
        }

        public void Put(ThreadUpdateRequest request)
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
            if (request == null || request.UserThreadId == null || userId == null)
                throw new ArgumentNullException();
            var blog = _blogService.GetBlogByShortname(request.BlogShortname, userId.GetValueOrDefault(), _blogRepository);
            var dto = new ThreadDto
            {
                UserThreadId = request.UserThreadId,
                PostId = request.PostId,
                BlogShortname = request.BlogShortname,
                UserBlogId = blog.UserBlogId ?? -1,
                UserTitle = request.UserTitle,
                WatchedShortname = request.WatchedShortname,
                ThreadTags = request.ThreadTags,
                IsArchived = request.IsArchived
            };
            _threadService.UpdateThread(dto, _threadRepository);
        }

        public void Delete([FromUri] int[] userThreadIds)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            foreach (var id in userThreadIds)
            {
                var thread = _threadService.GetById(id, _blogRepository, _threadRepository, _tumblrClient);
                var blog = _blogService.GetBlogById(thread.UserBlogId, _blogRepository);
                if (blog.UserId != user.UserId)
                    return;
                _threadService.DeleteThread(id, _threadRepository);
            }
        }
    }
}
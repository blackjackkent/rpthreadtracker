using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, false);
            foreach (var blog in blogs)
                ids.AddRange(_threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository, isArchived));
            return ids;
        }

        public HttpResponseMessage Post(ThreadDto thread)
        {
            var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
            if (thread == null || userId == null)
                throw new ArgumentNullException();
            var userOwnsBlog = _blogService.UserOwnsBlog(thread.UserBlogId, userId.GetValueOrDefault(), _blogRepository);
	        if (!userOwnsBlog)
	        {
		        return new HttpResponseMessage(HttpStatusCode.BadRequest);
	        }
            var dto = new ThreadDto
            {
                UserThreadId = null,
                PostId = thread.PostId,
                UserBlogId = thread.UserBlogId,
                UserTitle = thread.UserTitle,
                WatchedShortname = thread.WatchedShortname,
                ThreadTags = thread.ThreadTags
            };
            _threadService.AddNewThread(dto, _threadRepository);
			return new HttpResponseMessage(HttpStatusCode.Created);
		}

        public HttpResponseMessage Put(ThreadDto thread)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            if (thread == null || thread.UserThreadId == null || user == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
			bool userOwnsThread = _threadService.UserOwnsThread(user.UserId, thread.UserThreadId.GetValueOrDefault(), _threadRepository);
	        if (!userOwnsThread)
	        {
		        return new HttpResponseMessage(HttpStatusCode.BadRequest);
	        }
			_threadService.UpdateThread(thread, _threadRepository);
			return new HttpResponseMessage(HttpStatusCode.OK);
        }

		[Route("api/Thread/Delete")]
		[HttpPut]
        public void DeleteThreads(List<ThreadDto> threads)
        {
            var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity);
            foreach (var thread in threads)
            {
	            bool userOwnsThread = _threadService.UserOwnsThread(user.UserId, thread.UserThreadId.GetValueOrDefault(), _threadRepository);
                if (!userOwnsThread)
                    continue;
                _threadService.DeleteThread(thread.UserThreadId.GetValueOrDefault(), _threadRepository);
            }
        }
    }
}
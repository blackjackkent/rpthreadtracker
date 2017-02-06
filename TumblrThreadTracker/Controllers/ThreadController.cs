namespace TumblrThreadTracker.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using Models.DomainModels.Users;

	/// <summary>
	/// Controller class for getting and updating thread information
	/// </summary>
	[RedirectOnMaintenance]
	[Authorize]
	public class ThreadController : ApiController
	{
		private readonly IRepository<Blog> _blogRepository;
		private readonly IBlogService _blogService;
		private readonly IRepository<Thread> _threadRepository;
		private readonly IThreadService _threadService;
		private readonly IRepository<User> _userProfileRepository;
		private readonly ITumblrClient _tumblrClient;
		private readonly IWebSecurityService _webSecurityService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThreadController"/> class
		/// </summary>
		/// <param name="userBlogRepository">Unity-injected user blog repository</param>
		/// <param name="userThreadRepository">Unity-injected user thread repository</param>
		/// <param name="webSecurityService">Unity-injected web security service</param>
		/// <param name="blogService">Unity-injected blog service</param>
		/// <param name="threadService">Unity-injected thread service</param>
		/// <param name="tumblrClient">Unity-injected tumblr client</param>
		/// <param name="userProfileRepository">Unity-injected user profile repository</param>
		public ThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IWebSecurityService webSecurityService, IBlogService blogService, IThreadService threadService, ITumblrClient tumblrClient, IRepository<User> userProfileRepository)
		{
			_blogRepository = userBlogRepository;
			_threadRepository = userThreadRepository;
			_webSecurityService = webSecurityService;
			_blogService = blogService;
			_threadService = threadService;
			_tumblrClient = tumblrClient;
			_userProfileRepository = userProfileRepository;
		}

		/// <summary>
		/// Controller endpoint for removing threads from the database
		/// </summary>
		/// <param name="threads">Collection of <see cref="ThreadDto"/> objects to be removed</param>
		[Route("api/Thread/Delete")]
		[HttpPut]
		public void DeleteThreads(List<ThreadDto> threads)
		{
			var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity, _userProfileRepository);
			foreach (var thread in threads)
			{
				var userOwnsThread = _threadService.UserOwnsThread(user.UserId, thread.UserThreadId.GetValueOrDefault(), _threadRepository);
				if (userOwnsThread)
				{
					_threadService.DeleteThread(thread.UserThreadId.GetValueOrDefault(), _threadRepository);
				}
			}
		}

		/// <summary>
		/// Controller endpoint for getting a specific thread by UserThreadId
		/// </summary>
		/// <param name="id">Unique identifier of thread to be retrieved</param>
		/// <returns><see cref="ThreadDto"/> object describing requested blog</returns>
		public ThreadDto Get(int id)
		{
			return _threadService.GetById(id, _blogRepository, _threadRepository, _tumblrClient);
		}

		/// <summary>
		/// Controller endpoint for getting IDs for all threads belonging to currently authenticated user
		/// </summary>
		/// <param name="isArchived">Whether or not to retrieve archived threads</param>
		/// <returns>List of integer thread IDs</returns>
		public IEnumerable<int?> Get([FromUri] bool isArchived = false)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var ids = new List<int?>();
			var blogs = _blogService.GetBlogsByUserId(userId, _blogRepository, false);
			foreach (var blog in blogs)
			{
				ids.AddRange(_threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository, isArchived));
			}
			return ids;
		}

		/// <summary>
		/// Controller endpoint for adding a new thread to the currently authenticated user's account
		/// </summary>
		/// <param name="thread">Request body containing information about thread to be created</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		public HttpResponseMessage Post(ThreadDto thread)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (thread == null || userId == null)
			{
				throw new ArgumentNullException();
			}
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

		/// <summary>
		/// Controller endpoint for updating an existing thread
		/// </summary>
		/// <param name="thread"><see cref="ThreadDto"/> object containing information about thread to be updated</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		public HttpResponseMessage Put(ThreadDto thread)
		{
			var user = _webSecurityService.GetCurrentUserFromIdentity((ClaimsIdentity)User.Identity, _userProfileRepository);
			if (thread?.UserThreadId == null || user == null)
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			var userOwnsThread = _threadService.UserOwnsThread(user.UserId, thread.UserThreadId.GetValueOrDefault(), _threadRepository);
			if (!userOwnsThread)
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			_threadService.UpdateThread(thread, _threadRepository);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
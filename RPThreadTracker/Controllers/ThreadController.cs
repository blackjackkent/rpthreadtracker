namespace RPThreadTracker.Controllers
{
	using System.Collections.Generic;
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
		/// <returns>ActionResult object wrapping HTTP response</returns>
		[Route("api/Thread/Delete")]
		[HttpPut]
		public IHttpActionResult DeleteThreads(List<ThreadDto> threads)
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
			return Ok();
		}

		/// <summary>
		/// Controller endpoint for getting a specific thread by UserThreadId
		/// </summary>
		/// <param name="id">Unique identifier of thread to be retrieved</param>
		/// <returns><see cref="ThreadDto"/> object describing requested blog</returns>
		public IHttpActionResult Get(int id)
		{
			var thread = _threadService.GetById(id, _threadRepository);
			if (thread == null)
			{
				return NotFound();
			}
			var post = _tumblrClient.GetPost(thread.PostId, thread.BlogShortname);
			var hydratedThread = _threadService.HydrateThread(thread, post);
			return Ok(hydratedThread);
		}

		/// <summary>
		/// Controller endpoint for getting IDs for all threads belonging to currently authenticated user
		/// </summary>
		/// <param name="isArchived">Whether or not to retrieve archived threads</param>
		/// <param name="isHiatusedBlog">Whether or not to include threads belonging to blogs marked as on hiatus</param>
		/// <returns>List of integer thread IDs</returns>
		public IHttpActionResult Get([FromUri] bool isArchived = false, bool isHiatusedBlog = false)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var ids = _threadService.GetThreadIdsByUserId(userId, _threadRepository, isArchived, isHiatusedBlog);
			return Ok(ids);
		}

		/// <summary>
		/// Controller endpoint for adding a new thread to the currently authenticated user's account
		/// </summary>
		/// <param name="thread">Request body containing information about thread to be created</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		public IHttpActionResult Post(ThreadDto thread)
		{
			if (thread == null)
			{
				return BadRequest();
			}
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			var userOwnsBlog = _blogService.UserOwnsBlog(thread.UserBlogId, userId.GetValueOrDefault(), _blogRepository);
			if (!userOwnsBlog)
			{
				return BadRequest();
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
			var createdThread = _threadService.AddNewThread(dto, _threadRepository);
			return CreatedAtRoute("DefaultApi", new { id = createdThread.UserThreadId }, createdThread);
		}

		/// <summary>
		/// Controller endpoint for updating an existing thread
		/// </summary>
		/// <param name="thread"><see cref="ThreadDto"/> object containing information about thread to be updated</param>
		/// <returns>HttpResponseMessage indicating success or failure</returns>
		public IHttpActionResult Put(ThreadDto thread)
		{
			var userId = _webSecurityService.GetCurrentUserIdFromIdentity((ClaimsIdentity)User.Identity);
			if (thread?.UserThreadId == null)
			{
				return BadRequest();
			}
			var userOwnsThread = _threadService.UserOwnsThread(userId.GetValueOrDefault(), thread.UserThreadId.GetValueOrDefault(), _threadRepository);
			if (!userOwnsThread)
			{
				return BadRequest();
			}
			_threadService.UpdateThread(thread, _threadRepository);
			return Ok();
		}
	}
}
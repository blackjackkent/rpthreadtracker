namespace RPThreadTracker.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;

	/// <summary>
	/// Controller class for getting thread information available to unauthenticated users
	/// </summary>
	[RedirectOnMaintenance]
	public class PublicThreadController : ApiController
	{
		private readonly IRepository<Blog> _blogRepository;
		private readonly IBlogService _blogService;
		private readonly IRepository<Thread> _threadRepository;
		private readonly IThreadService _threadService;
		private readonly ITumblrClient _tumblrClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="PublicThreadController"/> class
		/// </summary>
		/// <param name="userBlogRepository">Unity-injected user blog repository</param>
		/// <param name="userThreadRepository">Unity-injected user thread repository</param>
		/// <param name="blogService">Unity-injected blog service</param>
		/// <param name="threadService">Unity-injected thread service</param>
		/// <param name="tumblrClient">Unity-injected Tumblr client</param>
		public PublicThreadController(IRepository<Blog> userBlogRepository, IRepository<Thread> userThreadRepository, IBlogService blogService, IThreadService threadService, ITumblrClient tumblrClient)
		{
			_blogRepository = userBlogRepository;
			_threadRepository = userThreadRepository;
			_blogService = blogService;
			_threadService = threadService;
			_tumblrClient = tumblrClient;
		}

		/// <summary>
		/// Controller endpoint for getting a specific thread by UserThreadId
		/// </summary>
		/// <param name="id">Unique identifier of thread to be retrieved</param>
		/// <returns><see cref="ThreadDto"/> object describing requested thread</returns>
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
		/// Controller endpoint for getting all thread IDs for a particular tracked blog
		/// </summary>
		/// <param name="userId">Unique identifier of user which owns blog</param>
		/// <param name="blogShortname">Shortname identifier of blog to be retrieved</param>
		/// <param name="isArchived">If true, retrieve archived threads; otherwise retrieve non-archived</param>
		/// <returns>Collection of integer identifiers for all relevant threads</returns>
		public IHttpActionResult Get(int userId, string blogShortname, bool isArchived = false)
		{
			if (string.IsNullOrEmpty(blogShortname))
			{
				return Ok(_threadService.GetThreadIdsByUserId(userId, _threadRepository, isArchived));
			}
			var blog = _blogService.GetBlogByShortname(blogShortname, userId, _blogRepository);
			if (blog == null)
			{
				return BadRequest();
			}
			var threads = _threadService.GetThreadsByBlog(blog, _threadRepository, isArchived);
			return Ok(threads.Select(t => t.UserThreadId));
		}
	}
}
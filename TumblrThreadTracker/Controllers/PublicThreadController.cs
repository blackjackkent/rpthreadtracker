namespace TumblrThreadTracker.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;

	using TumblrThreadTracker.Infrastructure.Filters;
	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Blogs;
	using TumblrThreadTracker.Models.DomainModels.Threads;

	[RedirectOnMaintenance]
	public class PublicThreadController : ApiController
	{
		private readonly IRepository<Blog> _blogRepository;

		private readonly IBlogService _blogService;

		private readonly IRepository<Thread> _threadRepository;

		private readonly IThreadService _threadService;

		private readonly ITumblrClient _tumblrClient;

		public PublicThreadController(
			IRepository<Blog> userBlogRepository,
			IRepository<Thread> userThreadRepository,
			IBlogService blogService,
			IThreadService threadService,
			ITumblrClient tumblrClient)
		{
			_blogRepository = userBlogRepository;
			_threadRepository = userThreadRepository;
			_blogService = blogService;
			_threadService = threadService;
			_tumblrClient = tumblrClient;
		}

		public ThreadDto Get(int id)
		{
			return _threadService.GetById(id, _blogRepository, _threadRepository, _tumblrClient);
		}

		public IEnumerable<int?> Get(int userId, string blogShortname)
		{
			var ids = new List<int?>();
			var blogs = !string.IsNullOrEmpty(blogShortname)
				            ? _blogService.GetBlogsByUserId(userId, _blogRepository, false)
					            .Where(b => b.BlogShortname == blogShortname)
					            .ToList()
				            : _blogService.GetBlogsByUserId(userId, _blogRepository, false).ToList();
			foreach (var blog in blogs) ids.AddRange(_threadService.GetThreadIdsByBlogId(blog.UserBlogId, _threadRepository));
			return ids;
		}
	}
}
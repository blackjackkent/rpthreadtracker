namespace RPThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Configuration;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;

	/// <inheritdoc cref="IThreadService"/>
	public class ThreadService : IThreadService
	{
		/// <inheritdoc cref="IThreadService"/>
		public ThreadDto AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository)
		{
			var createdThread = threadRepository.Insert(new Thread(threadDto));
			return createdThread.ToDto(createdThread.UserBlog.ToDto(), null);
		}

		/// <inheritdoc cref="IThreadService"/>
		public void DeleteThread(int userThreadId, IRepository<Thread> threadRepository)
		{
			threadRepository.Delete(userThreadId);
		}

		/// <inheritdoc cref="IThreadService"/>
		public ThreadDto GetById(int id, IRepository<Blog> blogRepository, IRepository<Thread> threadRepository, ITumblrClient tumblrClient, bool skipTumblrCall = false)
		{
			var thread = threadRepository.GetSingle(t => t.UserThreadId == id);
			var blog = blogRepository.GetSingle(b => b.UserBlogId == thread.UserBlogId).ToDto();
			if (skipTumblrCall)
			{
				return thread.ToDto(blog, null);
			}
			var post = tumblrClient.GetPost(thread.PostId, blog.BlogShortname);
			return thread.ToDto(blog, post);
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient)
		{
			var posts = tumblrClient.GetNewsPosts(5);
			return posts.Select(post => new ThreadDto
			{
				BlogShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
				IsMyTurn = false,
				LastPostDate = post.Timestamp,
				LastPostUrl = post.PostUrl,
				LastPosterShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
				PostId = post.Id.ToString(),
				UserTitle = post.Title
			}).ToList();
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<int?> GetThreadIdsByUserId(int? userId, IRepository<Thread> threadRepository, bool isArchived = false)
		{
			if (userId == null)
			{
				return new List<int?>();
			}
			var threads = threadRepository.Get(t => t.UserBlog != null && t.UserBlog.UserId == userId && t.IsArchived == isArchived);
			return threads.Select(t => t.UserThreadId).ToList();
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<ThreadDto> GetThreadsByBlog(BlogDto blog, IRepository<Thread> threadRepository, bool isArchived = false)
		{
			if (blog?.UserBlogId == null)
			{
				return new List<ThreadDto>();
			}
			var threads = threadRepository.Get(t => t.UserBlogId == blog.UserBlogId && t.IsArchived == isArchived);
			return threads.Select(t => t.ToDto(blog, null)).ToList();
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<int?> GetThreadIdsByBlog(BlogDto blog, IRepository<Thread> threadRepository, bool isArchived = false)
		{
			var threads = GetThreadsByBlog(blog, threadRepository, isArchived);
			return threads.Select(t => t.UserThreadId).ToList();
		}

		/// <inheritdoc cref="IThreadService"/>
		public void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository)
		{
			threadRepository.Update(dto.UserThreadId, new Thread(dto));
		}

		/// <inheritdoc cref="IThreadService"/>
		public bool UserOwnsThread(int userId, int threadId, IRepository<Thread> threadRepository)
		{
			var userOwnsThread = threadRepository.Get(t => t.UserThreadId == threadId && t.UserBlog != null && t.UserBlog.UserId == userId).FirstOrDefault();
			return userOwnsThread != null;
		}

		/// <inheritdoc cref="IThreadService"/>
		public Dictionary<int, IEnumerable<ThreadDto>> GetThreadDistribution(IEnumerable<BlogDto> blogs, IRepository<Thread> threadRepository, bool isArchived)
		{
			var distribution = new Dictionary<int, IEnumerable<ThreadDto>>();
			foreach (var blog in blogs)
			{
				var threads = GetThreadsByBlog(blog, threadRepository, isArchived).OrderBy(t => t.WatchedShortname).ThenBy(t => t.UserTitle);
				if (threads.Any())
				{
					distribution.Add(blog.UserBlogId.GetValueOrDefault(), threads);
				}
			}
			return distribution;
		}
	}
}
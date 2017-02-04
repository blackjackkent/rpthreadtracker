namespace TumblrThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Configuration;

	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Blogs;
	using TumblrThreadTracker.Models.DomainModels.Threads;

	public class ThreadService : IThreadService
	{
		public void AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository)
		{
			threadRepository.Insert(new Thread(threadDto));
		}

		public void DeleteThread(int userThreadId, IRepository<Thread> threadRepository)
		{
			threadRepository.Delete(userThreadId);
		}

		public IEnumerable<string> GetAllTagsByBlog(int? userBlogId, IRepository<Thread> threadRepository)
		{
			if (userBlogId == null) return new List<string>();
			var threads = threadRepository.Get(t => t.UserBlogId == userBlogId);
			return threads.SelectMany(t => t.ThreadTags);
		}

		public ThreadDto GetById(
			int id,
			IRepository<Blog> blogRepository,
			IRepository<Thread> threadRepository,
			ITumblrClient tumblrClient,
			bool skipTumblrCall = false)
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

		public IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient)
		{
			var posts = tumblrClient.GetNewsPosts(5);
			return
				posts.Select(
					post =>
						new ThreadDto
							{
								BlogShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
								ContentSnippet = null,
								IsMyTurn = false,
								LastPostDate = post.Timestamp,
								LastPostUrl = post.PostUrl,
								LastPosterShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
								PostId = post.Id.ToString(),
								Type = post.Type,
								UserTitle = post.Title
							}).ToList();
		}

		public IEnumerable<int?> GetThreadIdsByBlogId(
			int? blogId,
			IRepository<Thread> threadRepository,
			bool isArchived = false)
		{
			if (blogId == null) return new List<int?>();
			var threads = threadRepository.Get(t => t.UserBlogId == blogId && t.IsArchived == isArchived);
			return threads.Select(t => t.UserThreadId).ToList();
		}

		public IEnumerable<ThreadDto> GetThreadsByBlog(
			BlogDto blog,
			IRepository<Thread> threadRepository,
			bool isArchived = false)
		{
			if (blog?.UserBlogId == null) return new List<ThreadDto>();
			var threads = threadRepository.Get(t => t.UserBlogId == blog.UserBlogId && t.IsArchived == isArchived);
			return threads.Select(t => t.ToDto(blog, null)).ToList();
		}

		public void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository)
		{
			threadRepository.Update(dto.UserThreadId, new Thread(dto));
		}

		public bool UserOwnsThread(int userId, int threadId, IRepository<Thread> threadRepository)
		{
			var userOwnsThread =
				threadRepository.Get(t => t.UserThreadId == threadId && t.UserBlog != null && t.UserBlog.UserId == userId)
					.FirstOrDefault();
			return userOwnsThread != null;
		}
	}
}
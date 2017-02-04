namespace TumblrThreadTracker.Interfaces
{
	using System.Collections.Generic;

	using TumblrThreadTracker.Models.DomainModels.Blogs;
	using TumblrThreadTracker.Models.DomainModels.Threads;

	public interface IThreadService
	{
		void AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository);

		void DeleteThread(int userThreadId, IRepository<Thread> threadRepository);

		IEnumerable<string> GetAllTagsByBlog(int? userBlogId, IRepository<Thread> threadRepository);

		ThreadDto GetById(
			int id,
			IRepository<Blog> blogRepository,
			IRepository<Thread> threadRepository,
			ITumblrClient tumblrClient,
			bool skipTumblrCall = false);

		IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient);

		IEnumerable<int?> GetThreadIdsByBlogId(int? blogId, IRepository<Thread> threadRepository, bool isArchived = false);

		IEnumerable<ThreadDto> GetThreadsByBlog(BlogDto blog, IRepository<Thread> threadRepository, bool isArchived = false);

		void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository);

		bool UserOwnsThread(int userId, int threadId, IRepository<Thread> threadRepository);
	}
}
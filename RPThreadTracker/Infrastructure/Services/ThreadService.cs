namespace RPThreadTracker.Infrastructure.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Configuration;
	using Interfaces;
	using Models.DomainModels.Blogs;
	using Models.DomainModels.Threads;
	using Models.ServiceModels;

	/// <inheritdoc cref="IThreadService"/>
	public class ThreadService : IThreadService
	{
		/// <inheritdoc cref="IThreadService"/>
		public ThreadDto AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository)
		{
			var createdThread = threadRepository.Insert(new Thread(threadDto));
			return createdThread.ToDto();
		}

		/// <inheritdoc cref="IThreadService"/>
		public void DeleteThread(int userThreadId, IRepository<Thread> threadRepository)
		{
			threadRepository.Delete(userThreadId);
		}

		/// <inheritdoc cref="IThreadService"/>
		public ThreadDto GetById(int id, IRepository<Thread> threadRepository)
		{
			var thread = threadRepository.GetSingle(t => t.UserThreadId == id);
			return thread?.ToDto();
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient, IConfigurationService configurationService)
		{
			var posts = tumblrClient.GetNewsPosts(5);
			return posts.Select(post => new ThreadDto
			{
				BlogShortname = configurationService.NewsBlogShortname,
				IsMyTurn = false,
				LastPostDate = post.Timestamp,
				LastPostUrl = post.PostUrl,
				LastPosterShortname = configurationService.NewsBlogShortname,
				PostId = post.Id.ToString(),
				UserTitle = post.Title
			}).ToList();
		}

		/// <inheritdoc cref="IThreadService"/>
		public IEnumerable<int?> GetThreadIdsByUserId(int? userId, IRepository<Thread> threadRepository, bool isArchived = false, bool isHiatusedBlog = false)
		{
			if (userId == null)
			{
				return null;
			}
			var threads = threadRepository.Get(t => t.UserBlog != null
			                                        && t.UserBlog.UserId == userId
			                                        && t.IsArchived == isArchived
			                                        && t.UserBlog.OnHiatus == isHiatusedBlog);
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
			return threads.Select(t => t.ToDto()).ToList();
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
				var threads = threadRepository.Get(t => t.UserBlogId == blog.UserBlogId && t.IsArchived == isArchived);
				if (threads.Any())
				{
					distribution.Add(blog.UserBlogId.GetValueOrDefault(), threads.Select(t => t.ToDto()));
				}
			}
			return distribution;
		}

        /// <inheritdoc cref="IThreadService"/>
        public void MarkThreadQueued(int threadId, IRepository<Thread> threadRepository)
        {
            var thread = threadRepository.GetSingle(t => t.UserThreadId == threadId);
            thread.MarkedQueued = DateTime.UtcNow;
            threadRepository.Update(threadId, thread);
        }

		/// <inheritdoc cref="IThreadService"/>
		public ThreadDto HydrateThread(ThreadDto thread, IPost post, IRepository<Thread> threadRepository)
		{
			if (post == null)
			{
				return thread;
			}
			var mostRecentRelevantNote = post.GetMostRecentRelevantNote(thread.BlogShortname, thread.WatchedShortname);
			if (mostRecentRelevantNote == null)
			{
				HydrateLastPostInfoFromPost(thread, post);
				UnmarkThreadQueuedIfUpdated(thread, threadRepository);
				return thread;
			}
			HydrateLastPostInfoFromNote(thread, mostRecentRelevantNote);
			UnmarkThreadQueuedIfUpdated(thread, threadRepository);
			return thread;
		}

		/// <inheritdoc cref="IThreadService"/>
		public void ClearAllMarkedQueuedForUser(int userId, IRepository<Thread> threadRepository)
		{
			var relevantThreads = threadRepository.Get(t => t.MarkedQueued != null && t.UserBlog.UserId == userId).ToList();
			if (!relevantThreads.Any())
			{
				return;
			}
			foreach (var thread in relevantThreads)
			{
				thread.MarkedQueued = null;
				threadRepository.Update(thread.UserThreadId, thread);
			}
		}

		private void HydrateLastPostInfoFromNote(ThreadDto thread, Note note)
		{
			thread.LastPosterShortname = note.BlogName;
			thread.LastPostUrl = note.BlogUrl + "post/" + note.PostId;
			thread.LastPostDate = note.Timestamp;
			if (string.IsNullOrEmpty(thread.WatchedShortname))
			{
				thread.IsMyTurn = !string.Equals(note.BlogName, thread.BlogShortname, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				thread.IsMyTurn = string.Equals(note.BlogName, thread.WatchedShortname, StringComparison.OrdinalIgnoreCase);
			}
		}

		private void HydrateLastPostInfoFromPost(ThreadDto thread, IPost post)
		{
			thread.LastPosterShortname = post.BlogName;
			thread.LastPostUrl = post.PostUrl;
			thread.LastPostDate = post.Timestamp;
			if (string.IsNullOrEmpty(thread.WatchedShortname))
			{
				thread.IsMyTurn = !string.Equals(post.BlogName, thread.BlogShortname, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				thread.IsMyTurn = string.Equals(post.BlogName, thread.WatchedShortname, StringComparison.OrdinalIgnoreCase);
			}
		}

		private void UnmarkThreadQueuedIfUpdated(ThreadDto thread, IRepository<Thread> threadRepository)
		{
			if (thread.MarkedQueued == null || thread.LastPostDate == null)
			{
				return;
			}
			var lastPostDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
									.AddMilliseconds(thread.LastPostDate.GetValueOrDefault() * 1000);
			if (lastPostDate < thread.MarkedQueued)
			{
				return;
			}
			thread.MarkedQueued = null;
			threadRepository.Update(thread.UserThreadId, thread.ToModel());
		}
	}
}
namespace RPThreadTrackerTests.Helpers
{
	using System;
	using System.Collections.Generic;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Threads;

	public class ThreadBuilder
	{
		private string _blogShortname = "testblog";
		private bool _isArchived = false;
		private bool _isMyTurn = true;
		private long? _lastPostDate = DateTime.Now.Ticks;
		private string _lastPosterShortname = "mypartner";
		private string _lastPostUrl = "http://mypartner.tumblr.com/post/12345";
		private string _postId = "123456";
		private List<string> _threadTags = new List<string> { "tag1", "tag2" };
		private int _userBlogId = 1;
		private int? _userThreadId = 2;
		private string _userTitle = "Test Thread";
		private string _watchedShortname = "mypartner";

		public Thread Build()
		{
			return new Thread
			{
				UserBlogId = _userBlogId,
				PostId = _postId,
				UserThreadId = _userThreadId,
				IsArchived = _isArchived,
				ThreadTags = _threadTags,
				UserTitle = _userTitle,
				WatchedShortname = _watchedShortname,
				UserBlog = new Blog
				{
					UserBlogId = _userBlogId,
					BlogShortname = _blogShortname
				}
			};
		}

		public ThreadDto BuildDto()
		{
			return new ThreadDto
			{
				UserBlogId = _userBlogId,
				PostId = _postId,
				UserThreadId = _userThreadId,
				IsArchived = _isArchived,
				ThreadTags = _threadTags,
				UserTitle = _userTitle,
				WatchedShortname = _watchedShortname,
				BlogShortname = _blogShortname,
				IsMyTurn = _isMyTurn,
				LastPostDate = _lastPostDate,
				LastPostUrl = _lastPostUrl,
				LastPosterShortname = _lastPosterShortname
			};
		}

		public ThreadBuilder WithBlogShortname(string blogShortname)
		{
			_blogShortname = blogShortname;
			return this;
		}

		public ThreadBuilder WithIsArchived(bool isArchived)
		{
			_isArchived = isArchived;
			return this;
		}

		public ThreadBuilder WithIsMyTurn(bool isMyTurn)
		{
			_isMyTurn = isMyTurn;
			return this;
		}

		public ThreadBuilder WithLastPostDate(long? lastPostDate)
		{
			_lastPostDate = lastPostDate;
			return this;
		}

		public ThreadBuilder WithLastPosterShortname(string lastPosterShortname)
		{
			_lastPosterShortname = lastPosterShortname;
			return this;
		}

		public ThreadBuilder WithLastPostUrl(string lastPostUrl)
		{
			_lastPostUrl = lastPostUrl;
			return this;
		}

		public ThreadBuilder WithPostId(string postId)
		{
			_postId = postId;
			return this;
		}

		public ThreadBuilder WithThreadTags(List<string> threadTags)
		{
			_threadTags = threadTags;
			return this;
		}

		public ThreadBuilder WithUserBlogId(int userBlogId)
		{
			_userBlogId = userBlogId;
			return this;
		}

		public ThreadBuilder WithUserThreadId(int? userThreadId)
		{
			_userThreadId = userThreadId;
			return this;
		}

		public ThreadBuilder WithUserTitle(string userTitle)
		{
			_userTitle = userTitle;
			return this;
		}

		public ThreadBuilder WithWatchedShortname(string watchedShortname)
		{
			_watchedShortname = watchedShortname;
			return this;
		}
	}
}

namespace RPThreadTrackerTests.Infrastructure.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Infrastructure.Services;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Threads;
	using RPThreadTracker.Models.ServiceModels;

	[TestFixture]
	internal class ThreadServiceTests
	{
		private Mock<IRepository<Thread>> _threadRepository;
		private Mock<IConfigurationService> _configurationService;
		private Mock<ITumblrClient> _tumblrClient;
		private ThreadService _service;

		[SetUp]
		public void Setup()
		{
			_threadRepository = new Mock<IRepository<Thread>>();
			_configurationService = new Mock<IConfigurationService>();
			_tumblrClient = new Mock<ITumblrClient>();
			_service = new ThreadService();
		}

		[Test]
		public void AddNewThread_ReturnsThread()
		{
			// Arrange
			var thread = new ThreadBuilder()
				.WithUserThreadId(null)
				.BuildDto();
			var insertedThread = new ThreadBuilder()
				.WithUserBlogId(12345)
				.Build();
			_threadRepository.Setup(b => b.Insert(It.IsAny<Thread>())).Returns(insertedThread);

			// Act
			var result = _service.AddNewThread(thread, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(br => br.Insert(It.Is<Thread>(t => t.UserTitle == thread.UserTitle && t.UserBlogId == thread.UserBlogId)), Times.Once());
			Assert.That(result.UserBlogId, Is.EqualTo(insertedThread.UserBlogId));
		}

		[Test]
		public void DeleteThread_DeletesThread()
		{
			// Arrange
			int userThreadId = 12345;

			// Act
			_service.DeleteThread(userThreadId, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(t => t.Delete(userThreadId), Times.Once);
		}

		[Test]
		public void GetThreadById_ThreadNotFound_ReturnsNull()
		{
			// Arrange
			int userThreadId = 12345;
			_threadRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<Thread, bool>>>())).Returns((Thread)null);

			// Act
			var result = _service.GetById(userThreadId, _threadRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetThreadById_ThreadFound_ReturnsThreadDto()
		{
			// Arrange
			int userThreadId = 12345;
			var thread = new ThreadBuilder()
				.WithUserThreadId(userThreadId)
				.Build();
			_threadRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<Thread, bool>>>())).Returns(thread);

			// Act
			var result = _service.GetById(userThreadId, _threadRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.UserThreadId, Is.EqualTo(userThreadId));
			Assert.That(result.UserBlogId, Is.EqualTo(thread.UserBlogId));
		}

		[Test]
		public void GetNewsThreads_ReturnsDtoWithConfigShortname()
		{
			// Arrange
			int userThreadId = 12345;
			var newsBlogShortname = "testNewsBlog";
			var post = new Post
			{
				BlogName = "testBlog1",
				Date = DateTime.Now.ToString(),
				Id = 12345,
				PostUrl = "http://tempuri.org",
				Timestamp = DateTime.Now.Ticks,
				Title = "Test Title 1"
			};
			var post2 = new Post
			{
				BlogName = "testBlog2",
				Date = DateTime.Now.ToString(),
				Id = 23456,
				PostUrl = "http://tempuri2.org",
				Timestamp = DateTime.Now.Ticks,
				Title = "Test Title 2"
			};
			_tumblrClient.Setup(br => br.GetNewsPosts(It.IsAny<int>())).Returns(new List<IPost> { post, post2 });
			_configurationService.SetupGet(c => c.NewsBlogShortname).Returns(newsBlogShortname);

			// Act
			var result = _service.GetNewsThreads(_tumblrClient.Object, _configurationService.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(2));
			Assert.That(result.Count(t => t.BlogShortname == newsBlogShortname), Is.EqualTo(2));
			Assert.That(result.Any(t => t.PostId == post.Id.ToString()));
			Assert.That(result.Any(t => t.PostId == post2.Id.ToString()));
		}

		[Test]
		public void GetThreadIdsByUserId_UserIdNull_ReturnsNull()
		{
			// Act
			var result = _service.GetThreadIdsByUserId(null, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Never);
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetThreadIdsByUserId_UserIdNotNull_ReturnsThreadIds()
		{
			// Arrange
			var userId = 1234;
			var thread1 = new ThreadBuilder()
				.WithUserThreadId(1)
				.Build();
			var thread2 = new ThreadBuilder()
				.WithUserThreadId(2)
				.Build();
			var thread3 = new ThreadBuilder()
				.WithUserThreadId(3)
				.Build();
			var threadList = new List<Thread> { thread1, thread2, thread3 };
			_threadRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>())).Returns(threadList);

			// Act
			var result = _service.GetThreadIdsByUserId(userId, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Once);
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(3));
			Assert.That(result.Contains(1));
			Assert.That(result.Contains(2));
			Assert.That(result.Contains(3));
		}

		[Test]
		public void GetThreadsByBlog_BlogValid_ReturnsThreadDtos()
		{
			// Arrange
			var userId = 1234;
			var thread1 = new ThreadBuilder()
				.WithUserThreadId(1)
				.Build();
			var thread2 = new ThreadBuilder()
				.WithUserThreadId(2)
				.Build();
			var thread3 = new ThreadBuilder()
				.WithUserThreadId(3)
				.Build();
			var threadList = new List<Thread> { thread1, thread2, thread3 };
			var blog = new BlogBuilder()
				.WithUserBlogId(5)
				.BuildDto();
			_threadRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>())).Returns(threadList);

			// Act
			var result = _service.GetThreadsByBlog(blog, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Once);
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(3));
			Assert.That(result.Any(t => t.UserThreadId == 1));
			Assert.That(result.Any(t => t.UserThreadId == 2));
			Assert.That(result.Any(t => t.UserThreadId == 3));
		}

		[Test]
		public void GetThreadsByBlog_BlogNull_ReturnsEmpty()
		{
			// Act
			var result = _service.GetThreadsByBlog(null, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Never);
			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GetThreadsByBlog_BlogIdNull_ReturnsEmpty()
		{
			// Arrange
			var blog = new BlogBuilder()
				.WithUserBlogId(null)
				.BuildDto();

			// Act
			var result = _service.GetThreadsByBlog(blog, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Never);
			Assert.That(result, Is.Empty);
		}

		[Test]
		public void GetThreadIdsByBlog_BlogValid_ReturnsThreadIds()
		{
			// Arrange
			var userId = 1234;
			var thread1 = new ThreadBuilder()
				.WithUserThreadId(1)
				.Build();
			var thread2 = new ThreadBuilder()
				.WithUserThreadId(2)
				.Build();
			var thread3 = new ThreadBuilder()
				.WithUserThreadId(3)
				.Build();
			var threadList = new List<Thread> { thread1, thread2, thread3 };
			var blog = new BlogBuilder()
				.WithUserBlogId(5)
				.BuildDto();
			_threadRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>())).Returns(threadList);

			// Act
			var result = _service.GetThreadIdsByBlog(blog, _threadRepository.Object);

			// Assert
			_threadRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Thread, bool>>>()), Times.Once);
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count(), Is.EqualTo(3));
			Assert.That(result.Contains(1));
			Assert.That(result.Contains(2));
			Assert.That(result.Contains(3));
		}
	}
}

namespace RPThreadTrackerTests.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.Results;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Threads;

	internal class PublicThreadControllerTests
	{
		private PublicThreadController _publicThreadController;
		private Mock<IRepository<Blog>> _userBlogRepository;
		private Mock<IRepository<Thread>> _userThreadRepository;
		private Mock<IBlogService> _blogService;
		private Mock<IThreadService> _threadService;
		private Mock<ITumblrClient> _tumblrClient;

		[SetUp]
		public void Setup()
		{
			_userBlogRepository = new Mock<IRepository<Blog>>();
			_userThreadRepository = new Mock<IRepository<Thread>>();
			_blogService = new Mock<IBlogService>();
			_threadService = new Mock<IThreadService>();
			_tumblrClient = new Mock<ITumblrClient>();
			_publicThreadController = new PublicThreadController(_userBlogRepository.Object, _userThreadRepository.Object, _blogService.Object, _threadService.Object, _tumblrClient.Object);
		}

		[Test]
		public void Get_ThreadRequested_ReturnsHydratedThread()
		{
			// Arrange
			var threadId = 1;
			var thread = new ThreadBuilder()
				.WithUserThreadId(threadId)
				.BuildDto();
			var hydratedThread = new ThreadBuilder()
				.WithUserThreadId(threadId)
				.WithPostId("12345")
				.BuildDto();
			var post = new Mock<IPost>();
			_threadService.Setup(t => t.GetById(threadId, _userThreadRepository.Object)).Returns(thread);
			_tumblrClient.Setup(c => c.GetPost(thread.PostId, thread.BlogShortname)).Returns(post.Object);
			_threadService.Setup(s => s.HydrateThread(thread, post.Object)).Returns(hydratedThread);

			// Act
			var result = _publicThreadController.Get(threadId);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<ThreadDto>>());
			var content = result as OkNegotiatedContentResult<ThreadDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(hydratedThread));
		}

		[Test]
		public void Get_ThreadNotFound_ReturnsNotFound()
		{
			// Arrange
			var threadId = 1;
			_threadService.Setup(t => t.GetById(threadId, _userThreadRepository.Object)).Returns((ThreadDto)null);

			// Act
			var result = _publicThreadController.Get(threadId);

			// Assert
			Assert.That(result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void Get_ShortnameNotProvided_ReturnsAllUserThreadIds()
		{
			// Arrange
			var userId = 5;
			var threadIds = new List<int?> { 5, 10, 15 };
			_threadService.Setup(t => t.GetThreadIdsByUserId(userId, _userThreadRepository.Object, false)).Returns(threadIds);

			// Act
			var result = _publicThreadController.Get(userId, null);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<int?>>>());
			var content = result as OkNegotiatedContentResult<IEnumerable<int?>>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(threadIds));
		}

		[Test]
		public void Get_BlogNotFound_ReturnsBadRequest()
		{
			// Arrange
			var userId = 5;
			var shortname = "testBlog";
			_blogService.Setup(b => b.GetBlogByShortname(shortname, userId, _userBlogRepository.Object)).Returns((BlogDto)null);

			// Act
			var result = _publicThreadController.Get(userId, shortname);

			// Assert
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Get_ShortnameValid_ReturnsAllBlogThreadIds()
		{
			// Arrange
			var userId = 5;
			var shortname = "testBlog";
			var blog = new BlogBuilder().BuildDto();
			var thread1 = new ThreadBuilder()
				.WithUserThreadId(1)
				.BuildDto();
			var thread2 = new ThreadBuilder()
				.WithUserThreadId(2)
				.BuildDto();
			var thread3 = new ThreadBuilder()
				.WithUserThreadId(3)
				.BuildDto();
			_blogService.Setup(b => b.GetBlogByShortname(shortname, userId, _userBlogRepository.Object)).Returns(blog);
			_threadService.Setup(t => t.GetThreadsByBlog(blog, _userThreadRepository.Object, false)).Returns(new List<ThreadDto> { thread1, thread2, thread3 });

			// Act
			var result = _publicThreadController.Get(userId, shortname);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<int?>>>());
			var content = result as OkNegotiatedContentResult<IEnumerable<int?>>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content.Count(), Is.EqualTo(3));
			Assert.That(content.Content.Contains(1));
			Assert.That(content.Content.Contains(2));
			Assert.That(content.Content.Contains(3));
		}
	}
}

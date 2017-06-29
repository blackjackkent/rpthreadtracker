namespace RPThreadTrackerTests.Controllers
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using System.Web.Http.Results;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Threads;
	using RPThreadTracker.Models.DomainModels.Users;

	internal class ThreadControllerTests
	{
		private ThreadController _threadController;
		private Mock<IRepository<Blog>> _userBlogRepository;
		private Mock<IRepository<Thread>> _userThreadRepository;
		private Mock<IWebSecurityService> _webSecurityService;
		private Mock<IBlogService> _blogService;
		private Mock<IThreadService> _threadService;
		private Mock<ITumblrClient> _tumblrClient;
		private Mock<IRepository<User>> _userProfileRepository;

		[SetUp]
		public void Setup()
		{
			_userBlogRepository = new Mock<IRepository<Blog>>();
			_userThreadRepository = new Mock<IRepository<Thread>>();
			_webSecurityService = new Mock<IWebSecurityService>();
			_blogService = new Mock<IBlogService>();
			_threadService = new Mock<IThreadService>();
			_tumblrClient = new Mock<ITumblrClient>();
			_userProfileRepository = new Mock<IRepository<User>>();
			_threadController = new ThreadController(_userBlogRepository.Object, _userThreadRepository.Object, _webSecurityService.Object, _blogService.Object, _threadService.Object, _tumblrClient.Object, _userProfileRepository.Object);
		}

		[Test]
		public void Delete_RequestValid_UnownedThreadsSkipped()
		{
			// Arrange
			var currentUser = new UserBuilder()
				.WithUserId(1)
				.BuildDto();
			var threads = new List<ThreadDto>
			{
				new ThreadBuilder()
					.WithUserThreadId(1)
					.BuildDto(),
				new ThreadBuilder()
					.WithUserThreadId(2)
					.BuildDto()
			};
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(currentUser);
			_threadService.Setup(s => s.UserOwnsThread(currentUser.UserId, threads[0].UserThreadId.GetValueOrDefault(), _userThreadRepository.Object)).Returns(true);
			_threadService.Setup(s => s.UserOwnsThread(currentUser.UserId, threads[1].UserThreadId.GetValueOrDefault(), _userThreadRepository.Object)).Returns(false);

			// Act
			var result = _threadController.DeleteThreads(threads);

			// Assert
			_threadService.Verify(t => t.DeleteThread(It.IsAny<int>(), _userThreadRepository.Object), Times.Once);
			_threadService.Verify(t => t.DeleteThread(threads[0].UserThreadId.GetValueOrDefault(), _userThreadRepository.Object), Times.Once);
			_threadService.Verify(t => t.DeleteThread(threads[1].UserThreadId.GetValueOrDefault(), _userThreadRepository.Object), Times.Never);
			Assert.That(result, Is.TypeOf<OkResult>());
		}

		[Test]
		public async Task Get_ThreadRequested_ReturnsHydratedThread()
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
			_tumblrClient.Setup(c => c.GetPost(thread.PostId, thread.BlogShortname)).ReturnsAsync(post.Object);
			_threadService.Setup(s => s.HydrateThread(thread, post.Object, _userThreadRepository.Object)).Returns(hydratedThread);

			// Act
			var result = await _threadController.Get(threadId);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<ThreadDto>>());
			var content = result as OkNegotiatedContentResult<ThreadDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(hydratedThread));
		}

		[Test]
		public async Task Get_ThreadNotFound_ReturnsNotFound()
		{
			// Arrange
			var threadId = 1;
			_threadService.Setup(t => t.GetById(threadId, _userThreadRepository.Object)).Returns((ThreadDto)null);

			// Act
			var result = await _threadController.Get(threadId);

			// Assert
			Assert.That(result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void Get_ThreadsRequested_ReturnsThreadsIds()
		{
			// Arrange
			var userId = 5;
			var isArchived = false;
			var isHiatused = false;
			var threadList = new List<int?> { 5, 10, 15 };
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);
			_threadService.Setup(b => b.GetThreadIdsByUserId(userId, _userThreadRepository.Object, isArchived, isHiatused)).Returns(threadList);

			// Act
			var result = _threadController.Get(isArchived);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<int?>>>());
			var content = result as OkNegotiatedContentResult<IEnumerable<int?>>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(threadList));
		}

		[Test]
		public void Post_RequestNull_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _threadController.Post(null);

			// Assert
			_threadService.Verify(ts => ts.AddNewThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_UserDoesntOwnThreadBlog_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			var thread = new ThreadBuilder()
				.WithUserBlogId(2)
				.BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_blogService.Setup(s => s.UserOwnsBlog(thread.UserBlogId, currentUserId, _userBlogRepository.Object)).Returns(false);

			// Act
			var result = _threadController.Post(thread);

			// Assert
			_threadService.Verify(ts => ts.AddNewThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_RequestValid_CreatesThread()
		{
			// Arrange
			const int currentUserId = 5;
			var thread = new ThreadBuilder()
				.WithUserBlogId(2)
				.WithUserTitle("testThread")
				.BuildDto();
			var resultThread = new ThreadBuilder()
				.WithUserThreadId(15)
				.BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_blogService.Setup(s => s.UserOwnsBlog(thread.UserBlogId, currentUserId, _userBlogRepository.Object)).Returns(true);
			_threadService.Setup(t => t.AddNewThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object)).Returns(resultThread);

			// Act
			var result = _threadController.Post(thread);

			// Assert
			_threadService.Verify(ts => ts.AddNewThread(It.Is<ThreadDto>(t => t.UserTitle == thread.UserTitle && t.UserBlogId == thread.UserBlogId), _userThreadRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<CreatedAtRouteNegotiatedContentResult<ThreadDto>>());
			var content = result as CreatedAtRouteNegotiatedContentResult<ThreadDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(resultThread));
		}

		[Test]
		public void Put_RequestNull_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _threadController.Put(null);

			// Assert
			_threadService.Verify(ts => ts.UpdateThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_ThreadUserThreadIdNull_ReturnsBadRequest()
		{
			// Arrange
			var thread = new ThreadBuilder()
				.WithUserThreadId(null)
				.BuildDto();
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _threadController.Put(thread);

			// Assert
			_threadService.Verify(ts => ts.UpdateThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UserDoesNotOwnThread_ReturnsBadRequest()
		{
			// Arrange
			var thread = new ThreadBuilder().BuildDto();
			const int currentUserId = 4;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_threadService.Setup(t => t.UserOwnsThread(currentUserId, thread.UserThreadId.GetValueOrDefault(), _userThreadRepository.Object)).Returns(false);

			// Act
			var result = _threadController.Put(thread);

			// Assert
			_threadService.Verify(ts => ts.UpdateThread(It.IsAny<ThreadDto>(), _userThreadRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_RequestValid_UpdatesThread()
		{
			// Arrange
			var thread = new ThreadBuilder().BuildDto();
			var currentUserId = 4;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_threadService.Setup(t => t.UserOwnsThread(currentUserId, thread.UserThreadId.GetValueOrDefault(), _userThreadRepository.Object)).Returns(true);

			// Act
			var result = _threadController.Put(thread);

			// Assert
			_threadService.Verify(ts => ts.UpdateThread(thread, _userThreadRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}

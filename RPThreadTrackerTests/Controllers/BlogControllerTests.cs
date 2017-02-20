namespace RPThreadTrackerTests.Controllers
{
	using System.Collections.Generic;
	using System.Net;
	using System.Security.Claims;
	using System.Web.Http;
	using System.Web.Http.Results;
	using Builders;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;

	[TestFixture]
	internal class BlogControllerTests
	{
		private Mock<IWebSecurityService> _webSecurityService;
		private Mock<IBlogService> _blogService;
		private Mock<IRepository<Blog>> _blogRepository;
		private BlogController _blogController;

		[SetUp]
		public void Setup()
		{
			_blogRepository = new Mock<IRepository<Blog>>();
			_webSecurityService = new Mock<IWebSecurityService>();
			_blogService = new Mock<IBlogService>();
			_blogController = new BlogController(_blogRepository.Object, _webSecurityService.Object, _blogService.Object);
		}

		[Test]
		public void Delete_BlogNotNull_CallsDeleteMethod()
		{
			// Arrange
			const int userBlogId = 1;
			const int userId = 5;
			var blog = new BlogBuilder()
				.WithUserBlogId(userBlogId)
				.WithUserId(userId)
				.BuildDto();
			_blogService.Setup(b => b.GetBlogById(userBlogId, _blogRepository.Object)).Returns(blog);
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);

			// Act
			var result = _blogController.Delete(userBlogId);

			// Assert
			_blogService.Verify(b => b.DeleteBlog(userBlogId, _blogRepository.Object), Times.Once);
			Assert.That(result, Is.TypeOf<OkResult>());
		}

		[Test]
		public void Delete_UserDoesNotOwnBlog_ReturnsBadRequest()
		{
			// Arrange
			const int userBlogId = 1;
			const int currentUserId = 5;
			const int ownerUserId = 6;
			var blog = new BlogBuilder()
				.WithUserBlogId(1)
				.WithUserId(ownerUserId)
				.BuildDto();
			_blogService.Setup(b => b.GetBlogById(userBlogId, _blogRepository.Object)).Returns(blog);
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _blogController.Delete(userBlogId);

			// Assert
			_blogService.Verify(b => b.DeleteBlog(userBlogId, _blogRepository.Object), Times.Never);
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Delete_BlogNotFound_ReturnsBadRequest()
		{
			// Arrange
			const int userBlogId = 1;
			const int currentUserId = 5;
			_blogService.Setup(b => b.GetBlogById(userBlogId, _blogRepository.Object)).Returns((BlogDto)null);
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _blogController.Delete(userBlogId);

			// Assert
			_blogService.Verify(b => b.DeleteBlog(userBlogId, _blogRepository.Object), Times.Never);
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Get_BlogRequested_ReturnsBlog()
		{
			// Arrange
			var blogId = 1;
			var blog = new BlogBuilder()
				.WithUserBlogId(blogId)
				.BuildDto();
			_blogService.Setup(b => b.GetBlogById(blogId, _blogRepository.Object)).Returns(blog);

			// Act
			var result = _blogController.Get(blogId);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<BlogDto>>());
			var content = result as OkNegotiatedContentResult<BlogDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(blog));
		}

		[Test]
		public void Get_BlogNotFound_ReturnsNotFound()
		{
			// Arrange
			var blogId = 1;
			_blogService.Setup(b => b.GetBlogById(blogId, _blogRepository.Object)).Returns((BlogDto)null);

			// Act
			var result = _blogController.Get(blogId);

			// Assert
			Assert.That(result, Is.TypeOf<NotFoundResult>());
		}

		[Test]
		public void Get_BlogsRequested_ReturnsBlogs()
		{
			// Arrange
			var userId = 5;
			var includeHiatused = false;
			var blog1 = new BlogBuilder()
				.WithUserBlogId(1)
				.BuildDto();
			var blog2 = new BlogBuilder()
				.WithUserBlogId(2)
				.BuildDto();
			var blogList = new List<BlogDto> { blog1, blog2 };
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);
			_blogService.Setup(b => b.GetBlogsByUserId(userId, _blogRepository.Object, includeHiatused)).Returns(blogList);

			// Act
			var result = _blogController.Get(includeHiatused);

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<BlogDto>>>());
			var content = result as OkNegotiatedContentResult<IEnumerable<BlogDto>>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(blogList));
		}

		[Test]
		public void Post_UserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var blogShortname = "TestBlog";
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns((int?)null);

			// Act
			var result = _blogController.Post(blogShortname);

			// Assert
			_blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_ShortnameNull_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _blogController.Post(null);

			// Assert
			_blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_BlogExists_ReturnsBadRequest()
		{
			// Arrange
			const string blogShortname = "TestBlog";
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_blogService.Setup(b => b.UserIsTrackingShortname(blogShortname, currentUserId, _blogRepository.Object)).Returns(true);

			// Act
			var result = _blogController.Post(blogShortname);

			// Assert
			_blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_RequestValid_CreatesBlog()
		{
			// Arrange
			const string blogShortname = "TestBlog";
			const int currentUserId = 5;
			var blog = new BlogBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_blogService.Setup(b => b.UserIsTrackingShortname(blogShortname, currentUserId, _blogRepository.Object)).Returns(false);
			_blogService.Setup(b => b.AddNewBlog(It.IsAny<BlogDto>(), _blogRepository.Object)).Returns(blog);

			// Act
			var result = _blogController.Post(blogShortname);

			// Assert
			_blogService.Verify(bs => bs.AddNewBlog(It.Is<BlogDto>(b => b.BlogShortname == blogShortname && b.UserId == currentUserId), _blogRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<CreatedAtRouteNegotiatedContentResult<BlogDto>>());
			var content = result as CreatedAtRouteNegotiatedContentResult<BlogDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(blog));
		}

		[Test]
		public void Put_RequestNull_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _blogController.Put(null);

			// Assert
			_blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_BlogUserBlogIdNull_ReturnsBadRequest()
		{
			// Arrange
			var blog = new BlogBuilder()
				.WithUserBlogId(null)
				.BuildDto();
			const int currentUserId = 5;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);

			// Act
			var result = _blogController.Put(blog);

			// Assert
			_blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns((int?)null);

			// Act
			var result = _blogController.Put(blog);

			// Assert
			_blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UserDoesNotOwnBlog_ReturnsBadRequest()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			const int currentUserId = 4;
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			_blogService.Setup(b => b.UserOwnsBlog(blog.UserBlogId.GetValueOrDefault(), blog.UserId, _blogRepository.Object)).Returns(false);

			// Act
			var result = _blogController.Put(blog);

			// Assert
			_blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), _blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_RequestValid_UpdatesBlog()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(blog.UserId);
			_blogService.Setup(b => b.UserOwnsBlog(blog.UserBlogId.GetValueOrDefault(), blog.UserId, _blogRepository.Object)).Returns(true);

			// Act
			var result = _blogController.Put(blog);

			// Assert
			_blogService.Verify(bs => bs.UpdateBlog(blog, _blogRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}

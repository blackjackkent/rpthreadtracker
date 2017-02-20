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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			blogService.Setup(b => b.GetBlogById(userBlogId, blogRepository.Object)).Returns(blog);
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Delete(userBlogId);

			// Assert
			blogService.Verify(b => b.DeleteBlog(userBlogId, blogRepository.Object), Times.Once);
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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			blogService.Setup(b => b.GetBlogById(userBlogId, blogRepository.Object)).Returns(blog);
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Delete(userBlogId);

			// Assert
			blogService.Verify(b => b.DeleteBlog(userBlogId, blogRepository.Object), Times.Never);
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Delete_BlogNotFound_ReturnsBadRequest()
		{
			// Arrange
			const int userBlogId = 1;
			const int currentUserId = 5;
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			blogService.Setup(b => b.GetBlogById(userBlogId, blogRepository.Object)).Returns((BlogDto)null);
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Delete(userBlogId);

			// Assert
			blogService.Verify(b => b.DeleteBlog(userBlogId, blogRepository.Object), Times.Never);
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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			blogService.Setup(b => b.GetBlogById(blogId, blogRepository.Object)).Returns(blog);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Get(blogId);

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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			blogService.Setup(b => b.GetBlogById(blogId, blogRepository.Object)).Returns((BlogDto)null);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Get(blogId);

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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(userId);
			blogService.Setup(b => b.GetBlogsByUserId(userId, blogRepository.Object, includeHiatused)).Returns(blogList);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Get(includeHiatused);

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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns((int?)null);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Post(blogShortname);

			// Assert
			blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_ShortnameNull_ReturnsBadRequest()
		{
			// Arrange
			const int currentUserId = 5;
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Post(null);

			// Assert
			blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_BlogExists_ReturnsBadRequest()
		{
			// Arrange
			const string blogShortname = "TestBlog";
			const int currentUserId = 5;
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			blogService.Setup(b => b.UserIsTrackingShortname(blogShortname, currentUserId, blogRepository.Object)).Returns(true);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Post(blogShortname);

			// Assert
			blogService.Verify(bs => bs.AddNewBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Post_RequestValid_CreatesBlog()
		{
			// Arrange
			const string blogShortname = "TestBlog";
			const int currentUserId = 5;
			var blog = new BlogBuilder().BuildDto();
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			blogService.Setup(b => b.UserIsTrackingShortname(blogShortname, currentUserId, blogRepository.Object)).Returns(false);
			blogService.Setup(b => b.AddNewBlog(It.IsAny<BlogDto>(), blogRepository.Object)).Returns(blog);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Post(blogShortname);

			// Assert
			blogService.Verify(bs => bs.AddNewBlog(It.Is<BlogDto>(b => b.BlogShortname == blogShortname && b.UserId == currentUserId), blogRepository.Object), Times.Once());
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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Put(null);

			// Assert
			blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
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
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Put(blog);

			// Assert
			blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns((int?)null);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Put(blog);

			// Assert
			blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UserDoesNotOwnBlog_ReturnsBadRequest()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			const int currentUserId = 4;
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(currentUserId);
			blogService.Setup(b => b.UserOwnsBlog(blog.UserBlogId.GetValueOrDefault(), blog.UserId, blogRepository.Object)).Returns(false);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Put(blog);

			// Assert
			blogService.Verify(bs => bs.UpdateBlog(It.IsAny<BlogDto>(), blogRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_RequestValid_UpdatesBlog()
		{
			// Arrange
			var blog = new BlogBuilder().BuildDto();
			var blogRepository = new Mock<IRepository<Blog>>();
			var webSecurityService = new Mock<IWebSecurityService>();
			var blogService = new Mock<IBlogService>();
			webSecurityService.Setup(s => s.GetCurrentUserIdFromIdentity(It.IsAny<ClaimsIdentity>())).Returns(blog.UserId);
			blogService.Setup(b => b.UserOwnsBlog(blog.UserBlogId.GetValueOrDefault(), blog.UserId, blogRepository.Object)).Returns(true);
			var controller = new BlogController(blogRepository.Object, webSecurityService.Object, blogService.Object);

			// Act
			var result = controller.Put(blog);

			// Assert
			blogService.Verify(bs => bs.UpdateBlog(blog, blogRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}

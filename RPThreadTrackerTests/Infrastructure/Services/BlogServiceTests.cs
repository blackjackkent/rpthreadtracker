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

	[TestFixture]
	internal class BlogServiceTests
	{
		private Mock<IRepository<Blog>> _blogRepository;
		private BlogService _service;

		[SetUp]
		public void Setup()
		{
			_blogRepository = new Mock<IRepository<Blog>>();
			_service = new BlogService();
		}

		[Test]
		public void AddNewBlog_ReturnsBlog()
		{
			// Arrange
			var blog = new BlogBuilder()
				.WithUserBlogId(null)
				.BuildDto();
			var insertedBlog = new BlogBuilder()
				.WithUserBlogId(12345)
				.Build();
			_blogRepository.Setup(b => b.Insert(It.IsAny<Blog>())).Returns(insertedBlog);

			// Act
			var result = _service.AddNewBlog(blog, _blogRepository.Object);

			// Assert
			_blogRepository.Verify(br => br.Insert(It.Is<Blog>(b => b.UserId == blog.UserId && b.BlogShortname == blog.BlogShortname)), Times.Once());
			Assert.That(result.UserBlogId, Is.EqualTo(insertedBlog.UserBlogId));
		}

		[Test]
		public void DeleteBlog_DeletesBlog()
		{
			// Arrange
			int userBlogId = 12345;

			// Act
			_service.DeleteBlog(userBlogId, _blogRepository.Object);

			// Assert
			_blogRepository.Verify(b => b.Delete(userBlogId), Times.Once);
		}

		[Test]
		public void GetBlogById_BlogNotFound_ReturnsNull()
		{
			// Arrange
			int userBlogId = 12345;
			_blogRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<Blog, bool>>>())).Returns((Blog)null);

			// Act
			var result = _service.GetBlogById(userBlogId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetBlogById_BlogFound_ReturnsBlogDto()
		{
			// Arrange
			int userBlogId = 12345;
			var blog = new BlogBuilder()
				.WithUserBlogId(userBlogId)
				.Build();
			_blogRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(blog);

			// Act
			var result = _service.GetBlogById(userBlogId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.UserBlogId, Is.EqualTo(userBlogId));
			Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
		}

		[Test]
		public void GetBlogByShortname_BlogNotFound_ReturnsNull()
		{
			// Arrange
			int userId = 123;
			string shortname = "testShortname";
			_blogRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<Blog, bool>>>())).Returns((Blog)null);

			// Act
			var result = _service.GetBlogByShortname(shortname, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetBlogByShortname_BlogFound_ReturnsBlogDto()
		{
			// Arrange
			int userBlogId = 12345;
			int userId = 123;
			string shortname = "testShortname";
			var blog = new BlogBuilder()
				.WithUserBlogId(userBlogId)
				.WithUserId(userId)
				.WithBlogShortname(shortname)
				.Build();
			_blogRepository.Setup(r => r.GetSingle(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(blog);

			// Act
			var result = _service.GetBlogByShortname(shortname, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<BlogDto>());
			Assert.That(result.UserBlogId, Is.EqualTo(userBlogId));
			Assert.That(result.UserId, Is.EqualTo(userId));
			Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
		}

		[Test]
		public void GetBlogsByUserId_UserIdNull_ReturnsNull()
		{
			// Act
			var result = _service.GetBlogsByUserId(null, _blogRepository.Object, false);

			// Assert
			_blogRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>()), Times.Never);
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetBlogsByUserId_IncludeHiatusedBlogsTrue_DoesNotFilter()
		{
			// Arrange
			var userId = 1234;
			var blog1 = new BlogBuilder()
				.WithUserBlogId(1)
				.WithOnHiatus(false)
				.Build();
			var blog2 = new BlogBuilder()
				.WithUserBlogId(2)
				.WithOnHiatus(false)
				.Build();
			var blog3 = new BlogBuilder()
				.WithUserBlogId(3)
				.WithOnHiatus(true)
				.Build();
			var blogList = new List<Blog> { blog1, blog2, blog3 };
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(blogList);

			// Act
			var result = _service.GetBlogsByUserId(userId, _blogRepository.Object, true);

			// Assert
			_blogRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>()), Times.Once);
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<List<BlogDto>>());
			Assert.That(result.Count(), Is.EqualTo(3));
			Assert.That(result.Any(b => b.UserBlogId == 1));
			Assert.That(result.Any(b => b.UserBlogId == 2));
			Assert.That(result.Any(b => b.UserBlogId == 3));
		}

		[Test]
		public void GetBlogsByUserId_IncludeHiatusedBlogsFalse_DoesFilter()
		{
			// Arrange
			var userId = 1234;
			var blog1 = new BlogBuilder()
				.WithUserBlogId(1)
				.WithOnHiatus(false)
				.Build();
			var blog2 = new BlogBuilder()
				.WithUserBlogId(2)
				.WithOnHiatus(false)
				.Build();
			var blog3 = new BlogBuilder()
				.WithUserBlogId(3)
				.WithOnHiatus(true)
				.Build();
			var blogList = new List<Blog> { blog1, blog2, blog3 };
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(blogList);

			// Act
			var result = _service.GetBlogsByUserId(userId, _blogRepository.Object, false);

			// Assert
			_blogRepository.Verify(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>()), Times.Once);
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<List<BlogDto>>());
			Assert.That(result.Count(), Is.EqualTo(2));
			Assert.That(result.Any(b => b.UserBlogId == 1));
			Assert.That(result.Any(b => b.UserBlogId == 2));
			Assert.That(!result.Any(b => b.UserBlogId == 3));
		}

		[Test]
		public void UpdateBlog_UpdatesBlog()
		{
			// Arrange
			int userBlogId = 12345;
			var blog = new BlogBuilder()
				.WithUserBlogId(userBlogId)
				.BuildDto();

			// Act
			_service.UpdateBlog(blog, _blogRepository.Object);

			// Assert
			_blogRepository.Verify(br => br.Update(userBlogId, It.Is<Blog>(b => b.UserBlogId == userBlogId && b.BlogShortname == blog.BlogShortname)), Times.Once);
		}

		[Test]
		public void UserOwnsBlog_BlogNotFound_ReturnsFalse()
		{
			// Arrange
			var userId = 12345;
			var blogId = 1234;
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(new List<Blog>());

			// Act
			var result = _service.UserOwnsBlog(blogId, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void UserOwnsBlog_BlogFound_ReturnsTrue()
		{
			// Arrange
			var userId = 12345;
			var blogId = 1234;
			var blog = new BlogBuilder()
				.WithUserBlogId(blogId)
				.WithUserId(userId)
				.Build();
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(new List<Blog> { blog });

			// Act
			var result = _service.UserOwnsBlog(blogId, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void UserIsTrackingShortname_BlogNotFound_ReturnsFalse()
		{
			// Arrange
			var userId = 12345;
			var shortname = "testShortname";
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(new List<Blog>());

			// Act
			var result = _service.UserIsTrackingShortname(shortname, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void UserIsTrackingShortname_BlogFound_ReturnsTrue()
		{
			// Arrange
			var userId = 12345;
			var shortname = "testShortname";
			var blog = new BlogBuilder()
				.WithBlogShortname(shortname)
				.WithUserId(userId)
				.Build();
			_blogRepository.Setup(b => b.Get(It.IsAny<Expression<Func<Blog, bool>>>())).Returns(new List<Blog> { blog });

			// Act
			var result = _service.UserIsTrackingShortname(shortname, userId, _blogRepository.Object);

			// Assert
			Assert.That(result, Is.True);
		}
	}
}

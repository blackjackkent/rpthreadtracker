namespace RPThreadTrackerTests.Infrastructure.Services
{
	using System;
	using System.Linq.Expressions;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Infrastructure.Services;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Blogs;
	using RPThreadTracker.Models.DomainModels.Users;

	[TestFixture]
	internal class UserProfileServiceTests
	{
		private Mock<IRepository<User>> _userProfileRepository;
		private UserProfileService _service;

		[SetUp]
		public void Setup()
		{
			_userProfileRepository = new Mock<IRepository<User>>();
			_service = new UserProfileService();
		}

		[Test]
		public void UpdateUser_UpdatesUser()
		{
			// Arrange
			int userId = 12345;
			var user = new UserBuilder()
				.WithUserId(userId)
				.BuildDto();

			// Act
			_service.Update(user, _userProfileRepository.Object);

			// Assert
			_userProfileRepository.Verify(br => br.Update(userId, It.Is<User>(b => b.UserId == userId && b.UserName == user.UserName)), Times.Once);
		}

		[Test]
		public void GetUserByUsername_UserNotFound_ReturnsNull()
		{
			// Arrange
			int userId = 123;
			string username = "testUsername";
			_userProfileRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns((User)null);

			// Act
			var result = _service.GetUserByUsername(username, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetUserByUsername_UserFound_ReturnsUserDto()
		{
			// Arrange
			int userId = 12345;
			string username = "testUsername";
			var user = new UserBuilder()
				.WithUserId(userId)
				.WithUsername(username)
				.Build();
			_userProfileRepository.Setup(r => r.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

			// Act
			var result = _service.GetUserByUsername(username, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<UserDto>());
			Assert.That(result.UserId, Is.EqualTo(userId));
			Assert.That(result.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public void GetUserByEmail_UserNotFound_ReturnsNull()
		{
			// Arrange
			int userId = 123;
			string email = "test@test.com";
			_userProfileRepository.Setup(br => br.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns((User)null);

			// Act
			var result = _service.GetUserByEmail(email, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetUserByEmail_UserFound_ReturnsUserDto()
		{
			// Arrange
			int userId = 12345;
			string email = "test@test.com";
			var user = new UserBuilder()
				.WithUserId(userId)
				.WithEmail(email)
				.Build();
			_userProfileRepository.Setup(r => r.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

			// Act
			var result = _service.GetUserByEmail(email, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.TypeOf<UserDto>());
			Assert.That(result.UserId, Is.EqualTo(userId));
			Assert.That(result.Email, Is.EqualTo(user.Email));
		}
	}
}

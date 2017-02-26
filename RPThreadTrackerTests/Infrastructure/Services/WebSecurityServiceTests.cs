namespace RPThreadTrackerTests.Infrastructure.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Security.Claims;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Infrastructure.Services;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Account;
	using RPThreadTracker.Models.DomainModels.Users;

	[TestFixture]
	internal class WebSecurityServiceTests
	{
		private WebSecurityService _service;
		private Mock<IRepository<User>> _userProfileRepository;
		private Mock<IRepository<Membership>> _membershipRepository;
		private Mock<IEmailClient> _emailClient;
		private Mock<IConfigurationService> _configurationService;

		[SetUp]
		public void Setup()
		{
			_service = new WebSecurityService();
			_userProfileRepository = new Mock<IRepository<User>>();
			_membershipRepository = new Mock<IRepository<Membership>>();
			_emailClient = new Mock<IEmailClient>();
			_configurationService = new Mock<IConfigurationService>();
		}

		[Test]
		public void GetCurrentUserFromIdentity_IdentityNull_ReturnsNull()
		{
			// Act
			var result = _service.GetCurrentUserFromIdentity(null, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetCurrentUserFromIdentity_NoUserIdClaim_ReturnsNull()
		{
			// Arrange
			var identity = new ClaimsIdentity();

			// Act
			var result = _service.GetCurrentUserFromIdentity(identity, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetCurrentUserFromIdentity_ValidClaim_ReturnsUserDto()
		{
			// Arrange
			int userId = 12345;
			var claims = new List<Claim>
			{
				new Claim("userId", userId.ToString())
			};
			var identity = new ClaimsIdentity(claims);
			var user = new UserBuilder()
				.WithUserId(userId)
				.Build();
			_userProfileRepository.Setup(u => u.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

			// Act
			var result = _service.GetCurrentUserFromIdentity(identity, _userProfileRepository.Object);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.UserId, Is.EqualTo(userId));
			Assert.That(result.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public void GetCurrentUserIdFromIdentity_IdentityNull_ReturnsNull()
		{
			// Act
			var result = _service.GetCurrentUserIdFromIdentity(null);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetCurrentUserIdFromIdentity_NoUserIdClaim_ReturnsNull()
		{
			// Arrange
			var identity = new ClaimsIdentity();

			// Act
			var result = _service.GetCurrentUserIdFromIdentity(identity);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void GetCurrentUserIdFromIdentity_ValidClaim_ReturnsUserDto()
		{
			// Arrange
			int userId = 12345;
			var claims = new List<Claim>
			{
				new Claim("userId", userId.ToString())
			};
			var identity = new ClaimsIdentity(claims);
			var user = new UserBuilder()
				.WithUserId(userId)
				.Build();
			_userProfileRepository.Setup(u => u.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

			// Act
			var result = _service.GetCurrentUserIdFromIdentity(identity);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.EqualTo(userId));
		}
	}
}

namespace RPThreadTrackerTests.Controllers
{
	using System.Security.Claims;
	using System.Web.Http.Results;
	using Builders;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Users;
	using RPThreadTracker.Models.RequestModels;

	[TestFixture]
	internal class UserControllerTests
	{
		private Mock<IWebSecurityService> _webSecurityService;
		private Mock<IUserProfileService> _userProfileService;
		private Mock<IRepository<User>> _userProfileRepository;
		private UserController _userController;

		[SetUp]
		public void Setup()
		{
			_webSecurityService = new Mock<IWebSecurityService>();
			_userProfileService = new Mock<IUserProfileService>();
			_userProfileRepository = new Mock<IRepository<User>>();
			_userController = new UserController(_webSecurityService.Object, _userProfileService.Object, _userProfileRepository.Object);
		}

		[Test]
		public void Get_UserFound_ReturnsUser()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);

			// Act
			var result = _userController.Get();

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<UserDto>>());
			var content = result as OkNegotiatedContentResult<UserDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(user));
		}

		[Test]
		public void Post_EmailExists_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test"
			};
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);
			_userProfileService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), _userProfileRepository.Object)).Returns(true);
			_userProfileService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), _userProfileRepository.Object)).Returns(false);

			// Act
			var result = _userController.Post(request);

			// Assert
			_webSecurityService.Verify(s => s.CreateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), _userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestErrorMessageResult>());
			var response = result as BadRequestErrorMessageResult;
			Assert.That(response.Message, Is.EqualTo("An account with some or all of this information already exists."));
		}

		[Test]
		public void Post_UsernameExists_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test"
			};
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);
			_userProfileService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), _userProfileRepository.Object)).Returns(false);
			_userProfileService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), _userProfileRepository.Object)).Returns(true);

			// Act
			var result = _userController.Post(request);

			// Assert
			_webSecurityService.Verify(s => s.CreateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), _userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestErrorMessageResult>());
			var response = result as BadRequestErrorMessageResult;
			Assert.That(response.Message, Is.EqualTo("An account with some or all of this information already exists."));
		}

		[Test]
		public void Post_RequestValid_CreatesUser()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test",
				Password = "TestPassword",
				ConfirmPassword = "TestPassword"
			};
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);
			_userProfileService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), _userProfileRepository.Object)).Returns(false);
			_userProfileService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), _userProfileRepository.Object)).Returns(false);
			_webSecurityService.Setup(s => s.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository.Object)).Returns(user);

			// Act
			var result = _userController.Post(request);

			// Assert
			_webSecurityService.Verify(bs => bs.CreateAccount(request.Username, request.Password, request.Email, _userProfileRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<CreatedAtRouteNegotiatedContentResult<UserDto>>());
			var content = result as CreatedAtRouteNegotiatedContentResult<UserDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.EqualTo(user));
		}

		[Test]
		public void Put_RequestNull_ReturnsBadRequest()
		{
			// Act
			var result = _userController.Put(null);

			// Assert
			_userProfileService.Verify(us => us.Update(It.IsAny<UserDto>(), _userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_CurrentUserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns((UserDto)null);

			// Act
			var result = _userController.Put(user);

			// Assert
			_userProfileService.Verify(us => us.Update(It.IsAny<UserDto>(), _userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_UpdatedUserDoesntMatchCurrentUser_ReturnsBadRequest()
		{
			// Arrange
			var updatedUser = new UserBuilder()
				.WithUserId(1)
				.BuildDto();
			var currentUser = new UserBuilder()
				.WithUserId(2)
				.BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(currentUser);

			// Act
			var result = _userController.Put(updatedUser);

			// Assert
			_userProfileService.Verify(us => us.Update(It.IsAny<UserDto>(), _userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_RequestValid_UpdatesUser()
		{
			// Arrange
			var updatedUser = new UserBuilder()
				.WithUserId(1)
				.BuildDto();
			var currentUser = new UserBuilder()
				.WithUserId(1)
				.BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(currentUser);

			// Act
			var result = _userController.Put(updatedUser);

			// Assert
			_userProfileService.Verify(us => us.Update(updatedUser, _userProfileRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}

		[Test]
		public void ChangePassword_UserNotFound_ReturnsBadRequest()
		{
			// Arrange
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns((UserDto)null);

			// Act
			var result = _userController.ChangePassword(new ChangePasswordRequest());

			// Assert
			_webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void ChangePassword_UpdateFailed_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);
			_webSecurityService.Setup(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

			// Act
			var result = _userController.ChangePassword(new ChangePasswordRequest());

			// Assert
			_webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void ChangePassword_RequestValid_PerformsOperation()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			_webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), _userProfileRepository.Object)).Returns(user);
			_webSecurityService.Setup(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			// Act
			var result = _userController.ChangePassword(new ChangePasswordRequest());

			// Assert
			_webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}
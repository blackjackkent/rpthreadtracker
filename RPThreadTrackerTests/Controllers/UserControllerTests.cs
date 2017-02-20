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
		[Test]
		public void Get_UserFound_ReturnsUser()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Get();

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<UserDto>>());
			var content = result as OkNegotiatedContentResult<UserDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content, Is.Not.Null);
			Assert.That(content.Content.UserId, Is.EqualTo(user.UserId));
			Assert.That(content.Content.Email, Is.EqualTo(user.Email));
			Assert.That(content.Content.ShowDashboardThreadDistribution, Is.EqualTo(user.ShowDashboardThreadDistribution));
			Assert.That(content.Content.UseInvertedTheme, Is.EqualTo(user.UseInvertedTheme));
			Assert.That(content.Content.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public void Post_EmailExists_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test"
			};
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			userService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), userProfileRepository.Object)).Returns(true);
			userService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), userProfileRepository.Object)).Returns(false);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Post(request);

			// Assert
			webSecurityService.Verify(s => s.CreateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestErrorMessageResult>());
			var response = result as BadRequestErrorMessageResult;
			Assert.That(response.Message, Is.EqualTo("An account with some or all of this information already exists."));
		}

		[Test]
		public void Post_UsernameExists_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test"
			};
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			userService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), userProfileRepository.Object)).Returns(false);
			userService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), userProfileRepository.Object)).Returns(true);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Post(request);

			// Assert
			webSecurityService.Verify(s => s.CreateAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestErrorMessageResult>());
			var response = result as BadRequestErrorMessageResult;
			Assert.That(response.Message, Is.EqualTo("An account with some or all of this information already exists."));
		}

		[Test]
		public void Post_RequestValid_CreatesUser()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var request = new RegisterRequest
			{
				Email = user.Email,
				Username = "test",
				Password = "TestPassword",
				ConfirmPassword = "TestPassword"
			};
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			userService.Setup(s => s.UserExistsWithEmail(It.IsAny<string>(), userProfileRepository.Object)).Returns(false);
			userService.Setup(s => s.UserExistsWithUsername(It.IsAny<string>(), userProfileRepository.Object)).Returns(false);
			webSecurityService.Setup(s => s.CreateAccount(request.Username, request.Password, request.Email, userProfileRepository.Object)).Returns(user.ToDto());
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Post(request);

			// Assert
			webSecurityService.Verify(bs => bs.CreateAccount(request.Username, request.Password, request.Email, userProfileRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<CreatedAtRouteNegotiatedContentResult<UserDto>>());
			var content = result as CreatedAtRouteNegotiatedContentResult<UserDto>;
			Assert.That(content, Is.Not.Null);
			Assert.That(content.Content.UserId, Is.EqualTo(user.UserId));
			Assert.That(content.Content.Email, Is.EqualTo(user.Email));
			Assert.That(content.Content.ShowDashboardThreadDistribution, Is.EqualTo(user.ShowDashboardThreadDistribution));
			Assert.That(content.Content.UseInvertedTheme, Is.EqualTo(user.UseInvertedTheme));
			Assert.That(content.Content.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public void Put_RequestNull_ReturnsBadRequest()
		{
			// Arrange
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Put(null);

			// Assert
			userService.Verify(us => us.Update(It.IsAny<UserDto>(), userProfileRepository.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void Put_CurrentUserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns((User)null);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Put(user);

			// Assert
			userService.Verify(us => us.Update(It.IsAny<UserDto>(), userProfileRepository.Object), Times.Never());
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
				.Build();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(currentUser);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Put(updatedUser);

			// Assert
			userService.Verify(us => us.Update(It.IsAny<UserDto>(), userProfileRepository.Object), Times.Never());
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
				.Build();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(currentUser);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.Put(updatedUser);

			// Assert
			userService.Verify(us => us.Update(updatedUser, userProfileRepository.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}

		[Test]
		public void ChangePassword_UserNotFound_ReturnsBadRequest()
		{
			// Arrange
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns((User)null);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.ChangePassword(new ChangePasswordRequest());

			// Assert
			webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void ChangePassword_UpdateFailed_ReturnsBadRequest()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			webSecurityService.Setup(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.ChangePassword(new ChangePasswordRequest());

			// Assert
			webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public void ChangePassword_UpdateSucceeded_ReturnsOk()
		{
			// Arrange
			var user = new UserBuilder().Build();
			var webSecurityService = new Mock<IWebSecurityService>();
			var userService = new Mock<IUserProfileService>();
			var userProfileRepository = new Mock<IRepository<User>>();
			webSecurityService.Setup(s => s.GetCurrentUserFromIdentity(It.IsAny<ClaimsIdentity>(), userProfileRepository.Object)).Returns(user);
			webSecurityService.Setup(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
			var controller = new UserController(webSecurityService.Object, userService.Object, userProfileRepository.Object);

			// Act
			var result = controller.ChangePassword(new ChangePasswordRequest());

			// Assert
			webSecurityService.Verify(s => s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}
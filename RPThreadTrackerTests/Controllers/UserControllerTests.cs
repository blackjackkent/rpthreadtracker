namespace RPThreadTrackerTests.Controllers
{
	using System.Net;
	using System.Security.Claims;
	using System.Web.Http;
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
namespace RPThreadTrackerTests.Controllers
{
	using System.Threading.Tasks;
	using System.Web.Http.Results;
	using Builders;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Account;
	using RPThreadTracker.Models.DomainModels.Users;

	[TestFixture]
	internal class ForgotPasswordControllerTests
	{
		private Mock<IWebSecurityService> _webSecurityService;
		private Mock<IUserProfileService> _userProfileService;
		private Mock<IRepository<User>> _userProfileRepository;
		private Mock<IRepository<Membership>> _webpagesMembershipRepository;
		private Mock<IEmailService> _emailService;
		private ForgotPasswordController _forgotPasswordController;

		[SetUp]
		public void Setup()
		{
			_webSecurityService = new Mock<IWebSecurityService>();
			_userProfileService = new Mock<IUserProfileService>();
			_userProfileRepository = new Mock<IRepository<User>>();
			_webpagesMembershipRepository = new Mock<IRepository<Membership>>();
			_emailService = new Mock<IEmailService>();
			_forgotPasswordController = new ForgotPasswordController(_userProfileRepository.Object, _webpagesMembershipRepository.Object, _webSecurityService.Object, _emailService.Object, _userProfileService.Object);
		}

		[Test]
		public async Task Post_RequestNull_ReturnsBadRequest()
		{
			// Act
			var result = await _forgotPasswordController.Post(null);

			// Assert
			_webSecurityService.Verify(ws => ws.GeneratePasswordResetToken(It.IsAny<UserDto>()), Times.Never());
			_webSecurityService.Verify(ws => ws.SendForgotPasswordEmail(It.IsAny<UserDto>(), It.IsAny<string>(), _webpagesMembershipRepository.Object, _emailService.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public async Task Post_RequestNotValidUsernameOrEmail_ReturnsBadRequest()
		{
			// Arrange
			var usernameOrEmail = "invalidString";
			_userProfileService.Setup(u => u.GetUserByUsername(usernameOrEmail, _userProfileRepository.Object)).Returns((UserDto)null);
			_userProfileService.Setup(u => u.GetUserByEmail(usernameOrEmail, _userProfileRepository.Object)).Returns((UserDto)null);

			// Act
			var result = await _forgotPasswordController.Post(usernameOrEmail);

			// Assert
			_webSecurityService.Verify(ws => ws.GeneratePasswordResetToken(It.IsAny<UserDto>()), Times.Never());
			_webSecurityService.Verify(ws => ws.SendForgotPasswordEmail(It.IsAny<UserDto>(), It.IsAny<string>(), _webpagesMembershipRepository.Object, _emailService.Object), Times.Never());
			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		public async Task Post_RequestValidUsername_PerformsReset()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var token = "randomString";
			_userProfileService.Setup(u => u.GetUserByUsername(user.UserName, _userProfileRepository.Object)).Returns(user);
			_userProfileService.Setup(u => u.GetUserByEmail(user.UserName, _userProfileRepository.Object)).Returns((UserDto)null);
			_webSecurityService.Setup(s => s.GeneratePasswordResetToken(user)).Returns(token);

			// Act
			var result = await _forgotPasswordController.Post(user.UserName);

			// Assert
			_webSecurityService.Verify(ws => ws.GeneratePasswordResetToken(user), Times.Once());
			_webSecurityService.Verify(ws => ws.SendForgotPasswordEmail(user, token, _webpagesMembershipRepository.Object, _emailService.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}

		[Test]
		public async Task Post_RequestValidEmail_PerformsReset()
		{
			// Arrange
			var user = new UserBuilder().BuildDto();
			var token = "randomString";
			_userProfileService.Setup(u => u.GetUserByUsername(user.Email, _userProfileRepository.Object)).Returns((UserDto)null);
			_userProfileService.Setup(u => u.GetUserByEmail(user.Email, _userProfileRepository.Object)).Returns(user);
			_webSecurityService.Setup(s => s.GeneratePasswordResetToken(user)).Returns(token);

			// Act
			var result = await _forgotPasswordController.Post(user.Email);

			// Assert
			_webSecurityService.Verify(ws => ws.GeneratePasswordResetToken(user), Times.Once());
			_webSecurityService.Verify(ws => ws.SendForgotPasswordEmail(user, token, _webpagesMembershipRepository.Object, _emailService.Object), Times.Once());
			Assert.That(result, Is.TypeOf<OkResult>());
		}
	}
}
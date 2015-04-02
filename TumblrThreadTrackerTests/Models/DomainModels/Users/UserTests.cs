using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;
using TumblrThreadTrackerTests.TestBuilders;
using TumblrThreadTrackerTests.TestBuilders.Domain;

namespace TumblrThreadTrackerTests.Models.DomainModels.Users
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void New_User_Should_Correctly_Map_From_Dto()
        {
            // Arrange
            var dto = new UserProfileBuilder().BuildDto();

            // Act
            var user = dto.ToModel();

            // Assert
            Assert.That(user.UserName, Is.EqualTo(dto.UserName));
            Assert.That(user.Email, Is.EqualTo(dto.Email));
            Assert.That(user.UserId, Is.EqualTo(dto.UserId));
        }

        [Test]
        public void Send_Forgot_Password_Email_Should_Throw_Error_On_Invalid_Token()
        {
            // Arrange
            var repository = new Mock<IRepository<WebpagesMembership>>();
            repository.Setup(r => r.Get(It.IsAny<Expression<Func<WebpagesMembership, bool>>>()))
                .Returns(new List<WebpagesMembership>());
            var emailService = new Mock<IEmailService>();
            var securityService = new Mock<IWebSecurityService>();
            var user = new UserProfileBuilder().Build();

            // Act/Assert
            Assert.That(
                () =>
                    user.SendForgotPasswordEmail("testToken", repository.Object, emailService.Object,
                        securityService.Object),
                Throws.Exception.TypeOf<InvalidDataException>()
                );
        }

        [Test]
        public void Send_Forgot_Password_Email_Should_Throw_Error_On_Failed_Password_Reset()
        {
            // Arrange
            var repository = new Mock<IRepository<WebpagesMembership>>();
            repository.Setup(r => r.Get(It.IsAny<Expression<Func<WebpagesMembership, bool>>>()))
                .Returns(new List<WebpagesMembership> { new WebpagesMembership()});
            var emailService = new Mock<IEmailService>();
            var securityService = new Mock<IWebSecurityService>();
            securityService.Setup(s => s.ResetPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var user = new UserProfileBuilder().Build();

            // Act/Assert
            Assert.That(
                () =>
                    user.SendForgotPasswordEmail("testToken", repository.Object, emailService.Object,
                        securityService.Object),
                Throws.Exception.TypeOf<InvalidOperationException>()
                );
        }

        [Test]
        public void Send_Forgot_Password_Email_Should_Send_Email_To_User()
        {
            // Arrange
            var repository = new Mock<IRepository<WebpagesMembership>>();
            repository.Setup(r => r.Get(It.IsAny<Expression<Func<WebpagesMembership, bool>>>()))
                .Returns(new List<WebpagesMembership> { new WebpagesMembership() });
            var emailService = new Mock<IEmailService>();
            var securityService = new Mock<IWebSecurityService>();
            securityService.Setup(s => s.ResetPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var user = new UserProfileBuilder().Build();

            // Act
            user.SendForgotPasswordEmail("testToken", repository.Object, emailService.Object, securityService.Object);

            // Assert
            emailService.Verify(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once); 
        }

        [Test]
        public void UserProfile_Should_Correctly_Map_To_Dto()
        {
            // Arrange
            var userProfile = new UserProfileBuilder().Build();

            // Act
            var newDto = userProfile.ToDto();

            // Assert
            Assert.That(newDto.UserName, Is.EqualTo(userProfile.UserName));
            Assert.That(newDto.Email, Is.EqualTo(userProfile.Email));
            Assert.That(newDto.UserId, Is.EqualTo(userProfile.UserId));
        }
    }
}
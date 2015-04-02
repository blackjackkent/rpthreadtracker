using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Infrastructure.Repositories;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTrackerTests.TestBuilders;
using TumblrThreadTrackerTests.TestBuilders.Domain;

namespace TumblrThreadTrackerTests.Infrastructure.Services
{
    [TestFixture]
    public class UserProfileServiceTests
    {
        [Test]
        public void GetByUserIdShouldGetValidDto()
        {
            // Arrange
            var profile = new UserProfileBuilder().WithEmail("emailTest").Build();
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.Get(It.IsAny<int>())).Returns(profile);

            // Act
            var service = new UserProfileService();
            var result = service.GetByUserId(10, repository.Object);

            // Assert
            Assert.That(result.Email, Is.EqualTo(profile.Email));
            Assert.That(result.UserId, Is.EqualTo(profile.UserId));
            Assert.That(result.UserName, Is.EqualTo(profile.UserName));
        }

        [Test]
        public void GetByUserIdShouldHandleNulls()
        {
            // Arrange
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.Get(It.IsAny<int>())).Returns((UserProfile)null);

            // Act
            var service = new UserProfileService();
            var result = service.GetByUserId(10, repository.Object);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetByUsernameShouldGetValidDto()
        {
            // Arrange
            var profile = new UserProfileBuilder().WithEmail("emailTest").Build();
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.Get(It.IsAny<Expression<Func<UserProfile, bool>>>())).Returns(new List<UserProfile>{profile});

            // Act
            var service = new UserProfileService();
            var result = service.GetByUsername("test", repository.Object);

            // Assert
            Assert.That(result.Email, Is.EqualTo(profile.Email));
            Assert.That(result.UserId, Is.EqualTo(profile.UserId));
            Assert.That(result.UserName, Is.EqualTo(profile.UserName));
        }

        [Test]
        public void GetByUsernameShouldHandleNulls()
        {
            // Arrange
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.Get(It.IsAny<int>())).Returns((UserProfile)null);

            // Act
            var service = new UserProfileService();
            var result = service.GetByUsername("test", repository.Object);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}

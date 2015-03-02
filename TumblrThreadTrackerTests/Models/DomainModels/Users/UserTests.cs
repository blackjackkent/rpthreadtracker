using NUnit.Framework;
using TumblrThreadTracker.Models.DomainModels.Users;
using TumblrThreadTrackerTests.TestBuilders;

namespace TumblrThreadTrackerTests.Models.DomainModels.Users
{
    [TestFixture]
    public class UserTests
    {
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
    }
}
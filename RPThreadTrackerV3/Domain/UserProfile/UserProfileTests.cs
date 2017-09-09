namespace Domain.UserProfile
{
	using NUnit.Framework;

	[TestFixture]
    public class UserProfileTests
	{
		private UserProfile _userProfile;
		private const string UserId = "12345";
		private const string Username = "TestUser";
		private const string Email = "testemail@test.com";

		[SetUp]
		public void Setup()
		{
			_userProfile = new UserProfile();
		}

		[Test]
		public void CanGetAndSetUserId()
		{
			// act
			_userProfile.UserId = UserId;

			// assert
			Assert.That(_userProfile.UserId, Is.EqualTo(UserId));
		}

		[Test]
		public void CanGetAndSetUsername()
		{
			// act
			_userProfile.Username = Username;

			// assert
			Assert.That(_userProfile.Username, Is.EqualTo(Username));
		}

		[Test]
		public void CanGetAndSetEmail()
		{
			// act
			_userProfile.Email = Email;

			// assert
			Assert.That(_userProfile.Email, Is.EqualTo(Email));
		}
	}
}

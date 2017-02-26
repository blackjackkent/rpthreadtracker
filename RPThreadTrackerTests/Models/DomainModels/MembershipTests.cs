namespace RPThreadTrackerTests.Models.DomainModels
{
	using Helpers;
	using NUnit.Framework;

	[TestFixture]
	internal class MembershipTests
	{
		[Test]
		public void ToModel_ConstructsValidModel()
		{
			// Arrange
			var dto = new MembershipBuilder().BuildDto();

			// Act
			var result = dto.ToModel();

			// Assert
			Assert.That(result.UserId, Is.EqualTo(dto.UserId));
			Assert.That(result.PasswordVerificationToken, Is.EqualTo(dto.PasswordVerificationToken));
		}
	}
}

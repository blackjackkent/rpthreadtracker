namespace RPThreadTrackerTests.Models.DomainModels
{
	using Builders;
	using NUnit.Framework;

	[TestFixture]
	internal class UserTests
	{
		[Test]
		public void ToModel_ConstructsValidModel()
		{
			// Arrange
			var dto = new UserBuilder().BuildDto();

			// Act
			var result = dto.ToModel();

			// Assert
			Assert.That(result.UserId, Is.EqualTo(dto.UserId));
			Assert.That(result.Email, Is.EqualTo(dto.Email));
			Assert.That(result.ShowDashboardThreadDistribution, Is.EqualTo(dto.ShowDashboardThreadDistribution));
			Assert.That(result.UseInvertedTheme, Is.EqualTo(dto.UseInvertedTheme));
			Assert.That(result.UserName, Is.EqualTo(dto.UserName));
		}

		[Test]
		public void ToDto_ConstructsValidDto()
		{
			// Arrange
			var model = new UserBuilder().Build();

			// Act
			var result = model.ToDto();

			// Assert
			Assert.That(result.UserId, Is.EqualTo(model.UserId));
			Assert.That(result.Email, Is.EqualTo(model.Email));
			Assert.That(result.ShowDashboardThreadDistribution, Is.EqualTo(model.ShowDashboardThreadDistribution));
			Assert.That(result.UseInvertedTheme, Is.EqualTo(model.UseInvertedTheme));
			Assert.That(result.UserName, Is.EqualTo(model.UserName));
		}
	}
}

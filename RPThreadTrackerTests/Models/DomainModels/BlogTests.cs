namespace RPThreadTrackerTests.Models.DomainModels
{
	using Builders;
	using NUnit.Framework;

	[TestFixture]
	internal class BlogTests
	{
		[Test]
		public void ToModel_ConstructsValidModel()
		{
			// Arrange
			var dto = new BlogBuilder().BuildDto();

			// Act
			var result = dto.ToModel();

			// Assert
			Assert.That(result.UserId, Is.EqualTo(dto.UserId));
			Assert.That(result.BlogShortname, Is.EqualTo(dto.BlogShortname));
			Assert.That(result.OnHiatus, Is.EqualTo(dto.OnHiatus));
			Assert.That(result.UserBlogId, Is.EqualTo(dto.UserBlogId));
		}

		[Test]
		public void ToDto_ConstructsValidDto()
		{
			// Arrange
			var model = new BlogBuilder().Build();

			// Act
			var result = model.ToDto();

			// Assert
			Assert.That(result.UserId, Is.EqualTo(model.UserId));
			Assert.That(result.BlogShortname, Is.EqualTo(model.BlogShortname));
			Assert.That(result.OnHiatus, Is.EqualTo(model.OnHiatus));
			Assert.That(result.UserBlogId, Is.EqualTo(model.UserBlogId));
		}
	}
}

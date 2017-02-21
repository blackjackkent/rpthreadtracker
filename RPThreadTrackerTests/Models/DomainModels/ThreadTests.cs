namespace RPThreadTrackerTests.Models.DomainModels
{
	using Builders;
	using NUnit.Framework;

	[TestFixture]
	internal class ThreadTests
	{
		[Test]
		public void ToModel_ConstructsValidBaseModel()
		{
			// Arrange
			var dto = new ThreadBuilder().BuildDto();

			// Act
			var result = dto.ToModel();

			// Assert
			Assert.That(result.UserThreadId, Is.EqualTo(dto.UserThreadId));
			Assert.That(result.IsArchived, Is.EqualTo(dto.IsArchived));
			Assert.That(result.PostId, Is.EqualTo(dto.PostId));
			Assert.That(result.ThreadTags, Is.EqualTo(dto.ThreadTags));
			Assert.That(result.UserBlogId, Is.EqualTo(dto.UserBlogId));
			Assert.That(result.UserTitle, Is.EqualTo(dto.UserTitle));
			Assert.That(result.WatchedShortname, Is.EqualTo(dto.WatchedShortname));
		}

		[Test]
		public void ToDto_ConstructsValidBaseDto()
		{
			// Arrange
			var model = new ThreadBuilder().Build();

			// Act
			var result = model.ToDto();

			// Assert
			Assert.That(result.UserThreadId, Is.EqualTo(model.UserThreadId));
			Assert.That(result.IsArchived, Is.EqualTo(model.IsArchived));
			Assert.That(result.PostId, Is.EqualTo(model.PostId));
			Assert.That(result.ThreadTags, Is.EqualTo(model.ThreadTags));
			Assert.That(result.UserBlogId, Is.EqualTo(model.UserBlogId));
			Assert.That(result.UserTitle, Is.EqualTo(model.UserTitle));
			Assert.That(result.WatchedShortname, Is.EqualTo(model.WatchedShortname));
		}
	}
}

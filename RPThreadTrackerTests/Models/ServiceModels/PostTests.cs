using NUnit.Framework;
using RPThreadTracker.Models.ServiceModels;

namespace RPThreadTrackerTests.Models.ServiceModels
{
	[TestFixture]
	public class PostTests
	{
		[Test]
		public void MostRecentRelevantNote_NotesNull_ReturnsNull()
		{
			// Arrange
			var post = new Post
			{
				Notes = null
			};

			// Act
			var result = post.GetMostRecentRelevantNote("test", "test");

			// Assert
			Assert.That(result, Is.Null);
		}
	}
}

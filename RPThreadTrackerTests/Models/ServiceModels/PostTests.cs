namespace RPThreadTrackerTests.Models.ServiceModels
{
	using System;
	using System.Collections.Generic;
	using NUnit.Framework;
	using RPThreadTracker.Models.ServiceModels;

	[TestFixture]
	internal class PostTests
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

		[Test]
		public void MostRecentRelevantNote_NotesEmpty_ReturnsNull()
		{
			// Arrange
			var post = new Post
			{
				Notes = new List<Note>()
			};

			// Act
			var result = post.GetMostRecentRelevantNote("test", "test");

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void MostRecentRelevantNote_WatchedShortnameNullAndYourTurn_ReturnsMostRecentReblog()
		{
			// Arrange
			var date = DateTime.Now;
			var post = new Post
			{
				Notes = new List<Note>()
			};
			post.Notes.Add(new Note
			{
				Type = "like",
				BlogName = "partnerblog",
				PostId = "1",
				Timestamp = date.AddDays(-1).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerblog",
				PostId = "2",
				Timestamp = date.AddDays(-2).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "testblog",
				PostId = "3",
				Timestamp = date.AddDays(-3).Ticks
			});

			// Act
			var result = post.GetMostRecentRelevantNote("testblog", null);

			// Assert
			Assert.That(result.PostId, Is.EqualTo("2"));
		}

		[Test]
		public void MostRecentRelevantNote_WatchedShortnameNullAndTheirTurn_ReturnsMostRecentReblog()
		{
			// Arrange
			var date = DateTime.Now;
			var post = new Post
			{
				Notes = new List<Note>()
			};
			post.Notes.Add(new Note
			{
				Type = "like",
				BlogName = "partnerblog",
				PostId = "1",
				Timestamp = date.AddDays(-1).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "testblog",
				PostId = "2",
				Timestamp = date.AddDays(-2).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerblog",
				PostId = "3",
				Timestamp = date.AddDays(-3).Ticks
			});

			// Act
			var result = post.GetMostRecentRelevantNote("testblog", null);

			// Assert
			Assert.That(result.PostId, Is.EqualTo("2"));
		}

		[Test]
		public void MostRecentRelevantNote_WatchedShortnameNotNullAndTheirTurn_ReturnsUsersMostRecentReblog()
		{
			// Arrange
			var date = DateTime.Now;
			var post = new Post
			{
				Notes = new List<Note>()
			};
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "someOtherBlog",
				PostId = "1",
				Timestamp = date.AddDays(-1).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "testblog",
				PostId = "2",
				Timestamp = date.AddDays(-2).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerblog",
				PostId = "3",
				Timestamp = date.AddDays(-3).Ticks
			});

			// Act
			var result = post.GetMostRecentRelevantNote("testblog", "partnerblog");

			// Assert
			Assert.That(result.PostId, Is.EqualTo("2"));
		}

		[Test]
		public void MostRecentRelevantNote_WatchedShortnameNotNullAndYourTurn_ReturnsPartnersMostRecentReblog()
		{
			// Arrange
			var date = DateTime.Now;
			var post = new Post
			{
				Notes = new List<Note>()
			};
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "someOtherBlog",
				PostId = "1",
				Timestamp = date.AddDays(-1).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerblog",
				PostId = "2",
				Timestamp = date.AddDays(-2).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "testblog",
				PostId = "3",
				Timestamp = date.AddDays(-3).Ticks
			});

			// Act
			var result = post.GetMostRecentRelevantNote("testblog", "partnerblog");

			// Assert
			Assert.That(result.PostId, Is.EqualTo("2"));
		}

		[Test]
		public void MostRecentRelevantNote_WatchedShortnameNotNull_ReturnsCaseInsensitiveShortnameMatch()
		{
			// Arrange
			var date = DateTime.Now;
			var post = new Post
			{
				Notes = new List<Note>()
			};
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerBlog",
				PostId = "1",
				Timestamp = date.AddDays(-1).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "partnerblog",
				PostId = "2",
				Timestamp = date.AddDays(-2).Ticks
			});
			post.Notes.Add(new Note
			{
				Type = "reblog",
				BlogName = "testblog",
				PostId = "3",
				Timestamp = date.AddDays(-3).Ticks
			});

			// Act
			var result = post.GetMostRecentRelevantNote("testblog", "partnerblog");

			// Assert
			Assert.That(result.PostId, Is.EqualTo("1"));
		}
	}
}

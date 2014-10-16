using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TumblrThreadTracker.Models.Service_Models;

namespace TumblrThreadTrackerTests.Models.ServiceModels
{
    [TestFixture]
    public class PostTests
    {
        [Test]
        public void Post_Should_Get_Most_Recent_Reblog_When_WatchedShortname_Is_Not_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "reblog"
            };
            var post = new Post
            {
                notes = new List<Note> {note1, note2}
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, null);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("not-cmdr-blackjack-shepard"));
        }

        [Test]
        public void Post_Should_Get_Most_Recent_Reblog_And_Skip_Likes_When_Watched_Shortname_Is_Not_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "like"
            };
            var post = new Post
            {
                notes = new List<Note> { note1, note2 }
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, null);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("cmdr-blackjack-shepard"));
        }

        [Test]
        public void Post_Should_Get_Most_Recent_WatchedShortname_Post_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "reblog"
            };
            var note3 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 123456789,
                type = "reblog"
            };
            var post = new Post
            {
                notes = new List<Note> { note1, note2, note3 }
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";
            const string watchedShortname = "not-cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, watchedShortname);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("not-cmdr-blackjack-shepard"));
            Assert.That(returnedNote.timestamp, Is.EqualTo(123456789));
        }

        [Test]
        public void Post_Should_Get_Most_Recent_UserBlogName_Post_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "reblog"
            };
            var note3 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 123456789,
                type = "reblog"
            };
            var post = new Post
            {
                notes = new List<Note> { note1, note2, note3 }
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";
            const string watchedShortname = "not-cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, watchedShortname);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("cmdr-blackjack-shepard"));
            Assert.That(returnedNote.timestamp, Is.EqualTo(123456789));
        }

        [Test]
        public void Post_Should_Ignore_Irrelevant_Reblogs_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "reblog"
            };
            var note3 = new Note
            {
                blog_name = "somebody-else-entirely",
                timestamp = 123456789,
                type = "reblog"
            };
            var post = new Post
            {
                notes = new List<Note> { note1, note2, note3 }
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";
            const string watchedShortname = "not-cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, watchedShortname);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("not-cmdr-blackjack-shepard"));
            Assert.That(returnedNote.timestamp, Is.EqualTo(12345678));
        }

        [Test]
        public void Post_Should_Ignore_Likes_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var note1 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 1234567,
                type = "reblog"
            };
            var note2 = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                timestamp = 12345678,
                type = "reblog"
            };
            var note3 = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                timestamp = 123456789,
                type = "like"
            };
            var post = new Post
            {
                notes = new List<Note> { note1, note2, note3 }
            };
            const string userBlogShortname = "cmdr-blackjack-shepard";
            const string watchedShortname = "not-cmdr-blackjack-shepard";

            // Act
            var returnedNote = post.GetMostRecentRelevantNote(userBlogShortname, watchedShortname);

            // Assert
            Assert.That(returnedNote.blog_name, Is.EqualTo("not-cmdr-blackjack-shepard"));
            Assert.That(returnedNote.timestamp, Is.EqualTo(12345678));
        }
    }
}

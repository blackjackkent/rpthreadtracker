using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Domain.Blogs;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.Service_Models;
using Blog = TumblrThreadTracker.Domain.Blogs.Blog;

namespace TumblrThreadTrackerTests.Domain
{
    [TestFixture]
    public class ThreadTests
    {
        [Test]
        public void IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Not_Set()
        {
            // Arrange
            var blog = new Blog(new BlogDto { BlogShortname = "Cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5 });
            var post = new Mock<IPost>();
            var note = new Note
            {
                blog_name = "cmdr-blackjack-shepard",
                blog_url = "cmdr-blackjack-shepard",
                post_id = "12345",
                timestamp = 1234567,
                type = "reblog"
            };
            post.Setup(f => f.GetMostRecentRelevantNote(It.IsAny<string>(), It.IsAny<string>())).Returns(note);
            post.Setup(f => f.notes).Returns(new List<Note> {note});
            Thread thread = new Thread(new ThreadDto {UserThreadId = 15, UserBlogId = 10, PostId = 12345, UserTitle = "Test Thread", WatchedShortname = null});

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(false));
        }

        [Test]
        public void IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var blog = new Blog(new BlogDto { BlogShortname = "cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5 });
            var post = new Mock<IPost>();
            var note = new Note
            {
                blog_name = "not-cmdr-blackjack-shepard",
                blog_url = "not-cmdr-blackjack-shepard",
                post_id = "12345",
                timestamp = 1234567,
                type = "reblog"
            };
            post.Setup(f => f.GetMostRecentRelevantNote(It.IsAny<string>(), It.IsAny<string>())).Returns(note);
            post.Setup(f => f.notes).Returns(new List<Note> { note });
            Thread thread = new Thread(new ThreadDto { UserThreadId = 15, UserBlogId = 10, PostId = 12345, UserTitle = "Test Thread", WatchedShortname = "Not-cmdr-blackjack-shepard" });

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(true));
        }

        [Test]
        public void IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Not_Set_And_Notes_Are_Null()
        {
            // Arrange
            var blog = new Blog(new BlogDto { BlogShortname = "Cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5 });
            var post = new Mock<IPost>();
            post.Setup(f => f.notes).Returns((List<Note>)null);
            post.Setup(f => f.blog_name).Returns("cmdr-blackjack-shepard");
            Thread thread = new Thread(new ThreadDto { UserThreadId = 15, UserBlogId = 10, PostId = 12345, UserTitle = "Test Thread", WatchedShortname = null });

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(false));
        }
    }
}
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTracker.Models.ServiceModels;
using TumblrThreadTrackerTests.TestBuilders;
using TumblrThreadTrackerTests.TestBuilders.Domain;
using Blog = TumblrThreadTracker.Models.DomainModels.Blogs.Blog;

namespace TumblrThreadTrackerTests.Models.DomainModels.Threads
{
    [TestFixture]
    public class ThreadTests
    {
        [Test]
        public void IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Not_Set()
        {
            // Arrange
            var blog = new Blog(new BlogDto {BlogShortname = "Cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5});
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
            var thread =
                new Thread(new ThreadDto
                {
                    UserThreadId = 15,
                    UserBlogId = 10,
                    PostId = 12345,
                    UserTitle = "Test Thread",
                    WatchedShortname = null
                });

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(false));
        }

        [Test]
        public void
            IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Not_Set_And_Notes_Are_Null()
        {
            // Arrange
            var blog = new Blog(new BlogDto {BlogShortname = "Cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5});
            var post = new Mock<IPost>();
            post.Setup(f => f.notes).Returns((List<Note>) null);
            post.Setup(f => f.blog_name).Returns("cmdr-blackjack-shepard");
            var thread =
                new Thread(new ThreadDto
                {
                    UserThreadId = 15,
                    UserBlogId = 10,
                    PostId = 12345,
                    UserTitle = "Test Thread",
                    WatchedShortname = null
                });

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(false));
        }

        [Test]
        public void IsMyTurn_Should_Handle_Case_Insensitive_Shortnames_When_WatchedShortname_Is_Set()
        {
            // Arrange
            var blog = new Blog(new BlogDto {BlogShortname = "cmdr-blackjack-shepard", UserBlogId = 10, UserId = 5});
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
            post.Setup(f => f.notes).Returns(new List<Note> {note});
            var thread =
                new Thread(new ThreadDto
                {
                    UserThreadId = 15,
                    UserBlogId = 10,
                    PostId = 12345,
                    UserTitle = "Test Thread",
                    WatchedShortname = "Not-cmdr-blackjack-shepard"
                });

            // Act
            ThreadDto dto = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(dto.IsMyTurn, Is.EqualTo(true));
        }

        [Test]
        public void ThreadShouldMapToDtoCorrectlyWithNullPostObject()
        {
            // Arrange
            var blog = new BlogBuilder().Build();
            var thread = new ThreadBuilder().Build();
            IPost post = null;

            // Act
            var result = thread.ToDto(blog, post);

            // Assert
            Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(result.UserBlogId, Is.EqualTo(blog.UserBlogId));
            Assert.That(result.IsMyTurn, Is.EqualTo(true));
            Assert.That(result.LastPostDate, Is.Null);
            Assert.That(result.LastPostUrl, Is.Null);
            Assert.That(result.LastPosterShortname, Is.Null);
            Assert.That(result.PostId, Is.EqualTo(Convert.ToInt64(thread.PostId)));
            Assert.That(result.Type, Is.Null);
            Assert.That(result.UserThreadId, Is.EqualTo(thread.UserThreadId));
            Assert.That(result.UserTitle, Is.EqualTo(thread.UserTitle));
            Assert.That(result.WatchedShortname, Is.EqualTo(thread.WatchedShortname));

        }

        [Test]
        public void ThreadShouldMapToDtoCorrectlyWithNullUserBlogId()
        {
            // Arrange
            var blog = new BlogBuilder().WithUserBlogId(null).Build();
            var thread = new ThreadBuilder().Build();
            var post = new Mock<IPost>();
            post.Setup(p => p.id).Returns(15);
            post.Setup(p => p.type).Returns("test");
            post.Setup(p => p.GetMostRecentRelevantNote(It.IsAny<string>(), It.IsAny<string>())).Returns(new Note());

            // Act
            var result = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(result.UserThreadId, Is.EqualTo(thread.UserThreadId));
            Assert.That(result.PostId, Is.EqualTo(15));
            Assert.That(result.UserTitle, Is.EqualTo(thread.UserTitle));
            Assert.That(result.Type, Is.EqualTo("test"));
            Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(result.UserBlogId, Is.EqualTo(-1));
            Assert.That(result.WatchedShortname, Is.EqualTo(thread.WatchedShortname));
        }

        [Test]
        public void ThreadShouldMapToDtoCorrectlyWithNullPostObjectAndNullUserBlogId()
        {
            // Arrange
            var blog = new BlogBuilder().WithUserBlogId(null).Build();
            var thread = new ThreadBuilder().Build();
            IPost post = null;

            // Act
            var result = thread.ToDto(blog, post);

            // Assert
            Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(result.UserBlogId, Is.EqualTo(-1));
            Assert.That(result.IsMyTurn, Is.EqualTo(true));
            Assert.That(result.LastPostDate, Is.Null);
            Assert.That(result.LastPostUrl, Is.Null);
            Assert.That(result.LastPosterShortname, Is.Null);
            Assert.That(result.PostId, Is.EqualTo(Convert.ToInt64(thread.PostId)));
            Assert.That(result.Type, Is.Null);
            Assert.That(result.UserThreadId, Is.EqualTo(thread.UserThreadId));
            Assert.That(result.UserTitle, Is.EqualTo(thread.UserTitle));
            Assert.That(result.WatchedShortname, Is.EqualTo(thread.WatchedShortname));
        }

        [Test]
        public void ThreadShouldMapToDtoCorrectlyWhenMostRecentRelevantNoteIsNull()
        {
            // Arrange
            var blog = new BlogBuilder().WithUserBlogId(null).Build();
            var thread = new ThreadBuilder().Build();
            var post = new Mock<IPost>();
            post.Setup(p => p.id).Returns(15);
            post.Setup(p => p.type).Returns("test");
            post.Setup(p => p.GetMostRecentRelevantNote(It.IsAny<string>(), It.IsAny<string>())).Returns((Note)null);

            // Act
            var result = thread.ToDto(blog, post.Object);

            // Assert
            Assert.That(result.UserThreadId, Is.EqualTo(thread.UserThreadId));
            Assert.That(result.PostId, Is.EqualTo(15));
            Assert.That(result.UserTitle, Is.EqualTo(thread.UserTitle));
            Assert.That(result.Type, Is.EqualTo("test"));
            Assert.That(result.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(result.UserBlogId, Is.EqualTo(-1));
            Assert.That(result.WatchedShortname, Is.EqualTo(thread.WatchedShortname));
        }
    }
}
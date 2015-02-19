using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTrackerTests.Domain
{
    [TestFixture]
    public class BlogTests
    {
        [Test]
        public void New_Blog_Should_Correctly_Map_From_Dto()
        {
            // Arrange
            var dto = new BlogDto {BlogShortname = "testShortname", UserBlogId = 3, UserId = 7};

            // Act
            var blog = dto.ToModel();

            // Assert
            Assert.That(blog.BlogShortname, Is.EqualTo(dto.BlogShortname));
            Assert.That(blog.UserBlogId, Is.EqualTo(dto.UserBlogId));
            Assert.That(blog.UserId, Is.EqualTo(dto.UserId));
        }

        [Test]
        public void Blog_Should_Correctly_Map_To_Dto()
        {
            // Arrange
            var dto = new BlogDto { BlogShortname = "testShortname", UserBlogId = 3, UserId = 7 };
            var blog = dto.ToModel();

            // Act
            var newDto = blog.ToDto();

            // Assert
            Assert.That(dto.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(dto.UserBlogId, Is.EqualTo(blog.UserBlogId));
            Assert.That(dto.UserId, Is.EqualTo(blog.UserId));
        }
    }
}


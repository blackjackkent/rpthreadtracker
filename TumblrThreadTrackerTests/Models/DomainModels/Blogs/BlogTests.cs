using NUnit.Framework;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTrackerTests.TestBuilders;
using TumblrThreadTrackerTests.TestBuilders.Domain;

namespace TumblrThreadTrackerTests.Models.DomainModels.Blogs
{
    [TestFixture]
    public class BlogTests
    {
        [Test]
        public void New_Blog_Should_Correctly_Map_From_Dto()
        {
            // Arrange
            var dto = new BlogBuilder().BuildDto();

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
            var blog = new BlogBuilder().Build();

            // Act
            var newDto = blog.ToDto();

            // Assert
            Assert.That(newDto.BlogShortname, Is.EqualTo(blog.BlogShortname));
            Assert.That(newDto.UserBlogId, Is.EqualTo(blog.UserBlogId));
            Assert.That(newDto.UserId, Is.EqualTo(blog.UserId));
        }
    }
}


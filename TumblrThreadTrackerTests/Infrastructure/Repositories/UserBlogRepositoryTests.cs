using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TumblrThreadTracker.Infrastructure.Repositories;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTrackerTests.TestBuilders;

namespace TumblrThreadTrackerTests.Infrastructure.Repositories
{
    [TestFixture]
    public class UserBlogRepositoryTests
    {
        [Test]
        public void GetByIdShouldReturnSingleBlog()
        {
            // Arrange
            var blog = new BlogBuilder().WithUserBlogId(5).WithBlogShortname("test1");
            var blog2 = new BlogBuilder().WithUserBlogId(10).WithBlogShortname("test2");
            var blogs = new List<Blog>
            {
                blog, blog2
            };
            var context = new Mock<IThreadTrackerContext>();
            var set = GetQueryableMockDbSet<Blog>(blogs);
            context.SetupGet(c => c.UserBlogs).Returns(set);
            var repository = new UserBlogRepository(context.Object);

            // Act
            var result = repository.Get(10);

            // Assert
            Assert.That(result.UserBlogId, Is.EqualTo(10));
            Assert.That(result.BlogShortname, Is.EqualTo("test2"));
        }

        [Test]
        public void GetByCriteriaShouldReturnValidBlogs()
        {
            // Arrange
            var blog = new BlogBuilder().WithUserBlogId(5).WithBlogShortname("test1");
            var blog2 = new BlogBuilder().WithUserBlogId(10).WithBlogShortname("test2");
            var blog3 = new BlogBuilder().WithUserBlogId(11).WithBlogShortname("test2");
            var blogs = new List<Blog>
            {
                blog, blog2, blog3
            };
            var context = new Mock<IThreadTrackerContext>();
            var set = GetQueryableMockDbSet<Blog>(blogs);
            context.SetupGet(c => c.UserBlogs).Returns(set);
            var repository = new UserBlogRepository(context.Object);

            // Act
            var result = repository.Get(b => b.BlogShortname == "test2");

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(IEnumerable<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}

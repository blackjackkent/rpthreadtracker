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
using TumblrThreadTracker.Models.DomainModels.Threads;
using TumblrThreadTrackerTests.TestBuilders;

namespace TumblrThreadTrackerTests.Infrastructure.Repositories
{
    [TestFixture]
    public class UserThreadRepositoryTests
    {
        [Test]
        public void GetByIdShouldReturnSingleThread()
        {
            // Arrange
            var thread = new ThreadBuilder().WithUserThreadId(5);
            var thread2 = new ThreadBuilder().WithUserThreadId(10);
            var threads = new List<Thread>
            {
                thread, thread2
            };
            var context = new Mock<IThreadTrackerContext>();
            var set = GetQueryableMockDbSet<Thread>(threads);
            context.SetupGet(c => c.UserThreads).Returns(set);
            var repository = new UserThreadRepository(context.Object);

            // Act
            var result = repository.Get(10);

            // Assert
            Assert.That(result.UserThreadId, Is.EqualTo(10));
        }

        [Test]
        public void GetByCriteriaShouldReturnValidThreads()
        {
            // Arrange
            var thread = new ThreadBuilder().WithUserThreadId(5).WithUserTitle("test1");
            var thread2 = new ThreadBuilder().WithUserThreadId(10).WithUserTitle("test2");
            var thread3 = new ThreadBuilder().WithUserThreadId(11).WithUserTitle("test2");
            var threads = new List<Thread>
            {
                thread, thread2, thread3
            };
            var context = new Mock<IThreadTrackerContext>();
            var set = GetQueryableMockDbSet<Thread>(threads);
            context.SetupGet(c => c.UserThreads).Returns(set);
            var repository = new UserThreadRepository(context.Object);

            // Act
            var result = repository.Get(b => b.UserTitle == "test2");

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

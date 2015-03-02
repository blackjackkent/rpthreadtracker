using System.Collections.Generic;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Interfaces
{
    public interface IThreadService
    {
        IEnumerable<int?> GetThreadIdsByBlogId(int? blogId, IRepository<Thread> threadRepository);
        ThreadDto GetById(int id, IRepository<Blog> blogRepository, IRepository<Thread> threadRepository, ITumblrClient tumblrClient);
        void AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository);
        void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository);
        IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient);
        void DeleteThread(int userThreadId, IRepository<Thread> threadRepository);
    }
}
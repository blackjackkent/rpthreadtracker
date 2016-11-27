using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Interfaces
{
    public interface IExporterService
    {
        byte[] GetWorkbookBytes(bool includeArchived, int? userId, IEnumerable<BlogDto> blogs, IThreadService threadService, IRepository<Thread> threadRepository, IRepository<Blog> blogRepository);
    }
}
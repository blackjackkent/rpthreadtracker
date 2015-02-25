using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class ThreadService : IThreadService
    {
        public IEnumerable<int?> GetThreadIdsByBlogId(int? blogId, IRepository<Thread> threadRepository)
        {
            if (blogId == null)
                return new List<int?>();
            var threads = threadRepository.Get(t => t.UserBlogId == blogId);
            return threads.Select(t => t.UserThreadId);
        }

        public ThreadDto GetById(int id, IRepository<Blog> blogRepository, IRepository<Thread> threadRepository, ITumblrClient tumblrClient)
        {
            var thread = threadRepository.Get(id);
            var blog = blogRepository.Get(thread.UserBlogId);
            var post = tumblrClient.GetPost(thread.PostId, blog.BlogShortname);
            return thread.ToDto(blog, post);
        }

        public void AddNewThread(ThreadDto threadDto, IRepository<Thread> threadRepository)
        {
            threadRepository.Insert(new Thread(threadDto));
        }

        public void UpdateThread(ThreadDto dto, IRepository<Thread> threadRepository)
        {
            threadRepository.Update(new Thread(dto));
        }

        public IEnumerable<ThreadDto> GetNewsThreads(ITumblrClient tumblrClient)
        {
            var posts = tumblrClient.GetNewsPosts(5);
            return posts.Select(post => new ThreadDto
            {
                BlogShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
                ContentSnippet = null,
                IsMyTurn = false,
                LastPostDate = post.timestamp,
                LastPostUrl = post.post_url,
                LastPosterShortname = WebConfigurationManager.AppSettings["NewsBlogShortname"],
                PostId = Convert.ToInt64(post.id),
                Type = post.type,
                UserTitle = post.title
            }).ToList();
        }

        public void DeleteThread(int userThreadId, IRepository<Thread> threadRepository)
        {
            threadRepository.Delete(userThreadId);
        }
    }
}
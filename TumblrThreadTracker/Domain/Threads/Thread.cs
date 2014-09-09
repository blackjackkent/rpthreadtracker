using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.Service_Models;
using TumblrThreadTracker.Services;
using WebGrease.Css.Extensions;
using Blog = TumblrThreadTracker.Domain.Blogs.Blog;

namespace TumblrThreadTracker.Domain.Threads
{
    [Table("UserThread")]
    public class Thread
    {
        public Thread()
        {
        }

        public Thread(ThreadDto dto)
        {
            UserThreadId = dto.UserThreadId;
            UserBlogId = dto.UserBlogId;
            PostId = dto.PostId.ToString();
            UserTitle = dto.UserTitle;
            WatchedShortname = dto.WatchedShortname;
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? UserThreadId { get; set; }
        public int UserBlogId { get; set; }
        [ForeignKey("UserBlogId")]
        public Blog UserBlog { get; set; }
        public string PostId { get; set; }
        public string UserTitle { get; set; }
        public string WatchedShortname { get; set; }

        public static IEnumerable<int?> GetThreadIdsByBlogId(int? blogId, IUserThreadRepository threadRepository)
        {
            if (blogId == null)
            {
                return new List<int?>();
            }
            IEnumerable<Thread> threads = threadRepository.GetUserThreads(blogId);
            return threads.Select(t => t.UserThreadId);
        }

        public static ThreadDto GetById(int id, IUserBlogRepository blogRepository, IUserThreadRepository threadRepository)
        {
            Thread thread = threadRepository.GetUserThreadById(id);
            Blog blog = blogRepository.GetUserBlogById(thread.UserBlogId);
            Post post = ThreadService.GetPost(thread.PostId, blog.BlogShortname);
            return thread.ToDto(blog, post);
        }

        public static void AddNewThread(ThreadDto threadDto, IUserThreadRepository threadRepository)
        {
            threadRepository.InsertUserThread(new Thread(threadDto));
        }

        public static void UpdateThread(ThreadDto dto, IUserThreadRepository threadRepository)
        {
            threadRepository.UpdateUserThread(new Thread(dto));
        }

        public static IEnumerable<ThreadDto> GetNewsThreads()
        {
            List<ThreadDto> threads = new List<ThreadDto>();
            IEnumerable<Post> posts = ThreadService.GetNewsPosts(5);
            foreach (var post in posts)
            {
                var thread = new ThreadDto
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
                };
                threads.Add(thread);
            }
            return threads;
        }

        public ThreadDto ToDto(Blog blog, Post post)
        {
            if (post == null)
            {
                return new ThreadDto
                {
                    BlogShortname = blog.BlogShortname,
                    IsMyTurn = true,
                    LastPostDate = null,
                    LastPostUrl = null,
                    LastPosterShortname = null,
                    PostId = Convert.ToInt64(PostId),
                    Type = null,
                    UserThreadId = UserThreadId,
                    UserTitle = UserTitle,
                    WatchedShortname = WatchedShortname
                };
            }
            var dto = new ThreadDto
            {
                UserThreadId = UserThreadId,
                PostId = post.id,
                UserTitle = UserTitle,
                Type = post.type,
                BlogShortname = blog.BlogShortname,
                UserBlogId = blog.UserBlogId != null ? blog.UserBlogId.Value : -1,
                WatchedShortname = WatchedShortname
            };
            if (post.notes != null && post.notes.Any(n => n.type == "reblog"))
            {
                Note mostRecentNote = post.notes.OrderByDescending(n => n.timestamp).FirstOrDefault(n => n.type == "reblog");
                if (mostRecentNote != null)
                {
                    dto.LastPosterShortname = mostRecentNote.blog_name;
                    dto.LastPostUrl = mostRecentNote.blog_url + "post/" + mostRecentNote.post_id;
                    dto.LastPostDate = mostRecentNote.timestamp;
                    dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
                        ? mostRecentNote.blog_name == WatchedShortname
                        : mostRecentNote.blog_name != blog.BlogShortname;
                }
            }
            else
            {
                dto.LastPosterShortname = post.blog_name;
                dto.LastPostUrl = post.post_url;
                dto.LastPostDate = post.timestamp;
                dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
                        ? post.blog_name == WatchedShortname
                        : post.blog_name != blog.BlogShortname;
            }
            return dto;
        }
    }
}
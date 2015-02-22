﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Configuration;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Blogs;

namespace TumblrThreadTracker.Models.DomainModels.Threads
{
    [Table("UserThread")]
    public class Thread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserThreadId { get; set; }
        public int UserBlogId { get; set; }
        [ForeignKey("UserBlogId")]
        public Blog UserBlog { get; set; }
        public string PostId { get; set; }
        public string UserTitle { get; set; }
        public string WatchedShortname { get; set; }

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

        public ThreadDto ToDto(Blog blog, IPost post)
        {
            if (post == null)
            {
                return new ThreadDto
                {
                    BlogShortname = blog.BlogShortname,
                    UserBlogId = blog.UserBlogId.HasValue ? blog.UserBlogId.Value : -1,
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
                var mostRecentRelevantNote = post.GetMostRecentRelevantNote(blog.BlogShortname, WatchedShortname);

                if (mostRecentRelevantNote != null)
                {
                    dto.LastPosterShortname = mostRecentRelevantNote.blog_name;
                    dto.LastPostUrl = mostRecentRelevantNote.blog_url + "post/" + mostRecentRelevantNote.post_id;
                    dto.LastPostDate = mostRecentRelevantNote.timestamp;
                    dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
                        ? String.Equals(mostRecentRelevantNote.blog_name, WatchedShortname,
                            StringComparison.OrdinalIgnoreCase)
                        : !String.Equals(mostRecentRelevantNote.blog_name, blog.BlogShortname,
                            StringComparison.OrdinalIgnoreCase);
                    return dto;
                }
            }

            dto.LastPosterShortname = post.blog_name;
            dto.LastPostUrl = post.post_url;
            dto.LastPostDate = post.timestamp;
            dto.IsMyTurn = !string.IsNullOrEmpty(WatchedShortname)
                ? String.Equals(post.blog_name, WatchedShortname, StringComparison.OrdinalIgnoreCase)
                : !String.Equals(post.blog_name, blog.BlogShortname, StringComparison.OrdinalIgnoreCase);

            return dto;
        }
    }
}
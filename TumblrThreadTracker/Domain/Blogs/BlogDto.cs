using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Domain.Threads;

namespace TumblrThreadTracker.Domain.Blogs
{
    public class BlogDto
    {
        public int? UserBlogId { get; set; }
        public int UserId { get; set; }
        public string BlogShortname { get; set; }
    }
}
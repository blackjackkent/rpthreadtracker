using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.ViewModels
{
    public class ThreadManager
    {
        public int UserId { get; set; }
        public IEnumerable<UserBlog> UserBlogs { get; set; }
        public IEnumerable<Thread> Threads { get; set; }
    }
}
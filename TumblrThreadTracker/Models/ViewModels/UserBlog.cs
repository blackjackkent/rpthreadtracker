using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.ViewModels
{
    public class UserBlog
    {
        public int UserBlogId { get; set; }
        public int UserId { get; set; }
        public string BlogShortname { get; set; }
        public List<Thread> Threads { get; set; }
    }
}
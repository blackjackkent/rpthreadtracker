using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.Service_Models
{
    public class ServiceResponse
    {
        public Blog blog { get; set; }
        public List<Post> posts { get; set; }
        public long total_posts { get; set; }
    }
}
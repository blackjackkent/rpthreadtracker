using System.Collections.Generic;

namespace TumblrThreadTracker.Models.ServiceModels
{
    public class ServiceResponse
    {
        public Blog blog { get; set; }
        public List<Post> posts { get; set; }
        public long total_posts { get; set; }
    }
}
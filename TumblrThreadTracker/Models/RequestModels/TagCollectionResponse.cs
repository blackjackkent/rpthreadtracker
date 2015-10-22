using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.RequestModels
{
    public class TagCollectionResponse
    {
        public int? UserBlogId { get; set; }
        public string BlogShortname { get; set; }
        public IEnumerable<string> TagCollection { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TumblrThreadTracker.Models.Service_Models
{
    public class ServiceObject
    {
        public ServiceMeta meta { get; set; }
        public ServiceResponse response { get; set; }
    }
}
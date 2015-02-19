using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    public class NewsController : ApiController
    {
        public IEnumerable<ThreadDto> Get()
        {
            var threads = Thread.GetNewsThreads();
            return threads;
        }
    }
}
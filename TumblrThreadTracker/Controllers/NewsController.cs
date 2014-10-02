using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Domain.Threads;

namespace TumblrThreadTracker.Controllers
{
    public class NewsController : ApiController
    {
        public IEnumerable<ThreadDto> Get()
        {
            IEnumerable<ThreadDto> threads = Thread.GetNewsThreads();
            return threads;
        }
    }
}
using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    public class NewsController : ApiController
    {
        private readonly IThreadService _threadService;

        public NewsController(IThreadService threadService)
        {
            _threadService = threadService;
        }

        public IEnumerable<ThreadDto> Get()
        {
            var threads = _threadService.GetNewsThreads();
            return threads;
        }
    }
}
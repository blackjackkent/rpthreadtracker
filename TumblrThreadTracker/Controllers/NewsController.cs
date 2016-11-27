using System.Collections.Generic;
using System.Web.Http;
using TumblrThreadTracker.Infrastructure.Filters;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Threads;

namespace TumblrThreadTracker.Controllers
{
    [RedirectOnMaintenance]
    public class NewsController : ApiController
    {
        private readonly IThreadService _threadService;
        private readonly ITumblrClient _tumblrClient;

        public NewsController(IThreadService threadService, ITumblrClient tumblrClient)
        {
            _threadService = threadService;
            _tumblrClient = tumblrClient;
        }

        public IEnumerable<ThreadDto> Get()
        {
            var threads = _threadService.GetNewsThreads(_tumblrClient);
            return threads;
        }
    }
}
namespace TumblrThreadTracker.Controllers
{
	using System.Collections.Generic;
	using System.Web.Http;
	using Infrastructure.Filters;
	using Interfaces;
	using Models.DomainModels.Threads;

	/// <summary>
	/// Controller class for getting recent updates from the tracker news blog
	/// </summary>
	[RedirectOnMaintenance]
	public class NewsController : ApiController
	{
		private readonly IThreadService _threadService;
		private readonly ITumblrClient _tumblrClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="NewsController"/> class
		/// </summary>
		/// <param name="threadService">Unity-injected thread service</param>
		/// <param name="tumblrClient">Unity-injected Tumblr client</param>
		public NewsController(IThreadService threadService, ITumblrClient tumblrClient)
		{
			_threadService = threadService;
			_tumblrClient = tumblrClient;
		}

		/// <summary>
		/// Controller endpoint to retrieve recent posts from news blog
		/// </summary>
		/// <returns>List of <see cref="ThreadDto"/> objects representing recent posts</returns>
		public IEnumerable<ThreadDto> Get()
		{
			var threads = _threadService.GetNewsThreads(_tumblrClient);
			return threads;
		}
	}
}
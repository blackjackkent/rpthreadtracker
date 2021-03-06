﻿namespace RPThreadTracker.Controllers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
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
		private readonly IConfigurationService _configurationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="NewsController"/> class
		/// </summary>
		/// <param name="threadService">Unity-injected thread service</param>
		/// <param name="tumblrClient">Unity-injected Tumblr client</param>
		/// <param name="configurationService">Unity-injected configuration service</param>
		public NewsController(IThreadService threadService, ITumblrClient tumblrClient, IConfigurationService configurationService)
		{
			_threadService = threadService;
			_tumblrClient = tumblrClient;
			_configurationService = configurationService;
		}

		/// <summary>
		/// Controller endpoint to retrieve recent posts from news blog
		/// </summary>
		/// <returns>List of <see cref="ThreadDto"/> objects representing recent posts</returns>
		public async Task<IHttpActionResult> Get()
		{
			var threads = await _threadService.GetNewsThreads(_tumblrClient, _configurationService).ConfigureAwait(false);
			return Ok(threads);
		}
	}
}
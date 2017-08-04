namespace RPThreadTracker.Infrastructure.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using DontPanic.TumblrSharp;
	using DontPanic.TumblrSharp.Client;
	using DontPanic.TumblrSharp.OAuth;
	using Filters;
	using Interfaces;
	using log4net;

	/// <inheritdoc cref="ITumblrClient"/>
	[ExcludeFromCoverage]
	public class TumblrSharpClient : ITumblrClient
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(TumblrSharpClient));
		private readonly IConfigurationService _configurationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="TumblrSharpClient"/> class
		/// </summary>
		/// <param name="configurationService">Unity-injected configuration service</param>
		public TumblrSharpClient(IConfigurationService configurationService)
		{
			_configurationService = configurationService;
		}

		/// <inheritdoc cref="ITumblrClient"/>
		public async Task<IEnumerable<IPost>> GetNewsPosts(int? limit = null)
		{
			var posts = await RetrieveApiData(null, _configurationService.NewsBlogShortname, "news", limit);
			return posts;
		}

		/// <inheritdoc cref="ITumblrClient"/>
		public async Task<IPost> GetPost(string postId, string blogShortname)
		{
			if (string.IsNullOrWhiteSpace(postId))
			{
				return null;
			}
			var posts = await RetrieveApiData(postId, blogShortname);
			if (posts != null && posts.Any(p => p != null))
			{
				return posts.FirstOrDefault();
			}
			RefreshApiCache(postId, blogShortname);
			var updatedPosts = await RetrieveApiData(postId, blogShortname);
			if (updatedPosts == null)
			{
				return null;
			}
			return updatedPosts.FirstOrDefault();
		}

		private static void RefreshApiCache(string postId, string blogShortname)
		{
			var url = "http://" + blogShortname + ".tumblr.com/post/" + postId;
			var webRequest = WebRequest.Create(url);
			try
			{
				webRequest.GetResponse().GetResponseStream();
			}
			catch
			{
				// honestly I don't care if it 404ed
			}
		}

		private async Task<IEnumerable<IPost>> RetrieveApiData(string postId, string blogShortname, string tag = null, int? limit = null)
		{
			var factory = new TumblrClientFactory();
			var adapter = new TumblrSharpPostAdapter();
			var token = new Token(_configurationService.TumblrOauthToken, _configurationService.TumblrOauthSecret);
			using (var client = factory.Create<DontPanic.TumblrSharp.Client.TumblrClient>(_configurationService.TumblrConsumerKey, _configurationService.TumblrConsumerSecret, token))
			{
				var parameters = new MethodParameterSet();
				parameters.Add("notes_info", true);
				if (postId != null)
				{
					parameters.Add("id", postId);
				}
				if (tag != null)
				{
					parameters.Add("tag", tag);
				}
				if (limit != null)
				{
					parameters.Add("limit", limit.GetValueOrDefault(), 0);
				}
				try
				{
					var posts = await client.CallApiMethodAsync<Posts>(
						new BlogMethod(blogShortname, "posts/text", client.OAuthToken, HttpMethod.Get, parameters),
						CancellationToken.None);
					return posts.Result.Select(p => adapter.GetPost(p)).ToList();
				}
				catch (Exception e)
				{
					var test = e;
					Logger.Error($"TumblrSharpClient.RetrieveApiData: Error retrieving post with ID {postId} and blog shortname {blogShortname}: {e.Message} (");
					return null;
				}
			}
		}
	}
}
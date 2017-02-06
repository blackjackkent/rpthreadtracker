namespace TumblrThreadTracker.Infrastructure.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Configuration;
	using Interfaces;
	using Models.ServiceModels;
	using RestSharp;

	/// <inheritdoc cref="ITumblrClient"/>
	public class TumblrClient : ITumblrClient
	{
		private const string ApiKey = "***REMOVED***";
		private static IRestClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="TumblrClient"/> class
		/// </summary>
		/// <param name="client">Unity-injected HTTP client</param>
		public TumblrClient(IRestClient client)
		{
			_client = client;
		}

		/// <inheritdoc cref="ITumblrClient"/>
		public IEnumerable<IPost> GetNewsPosts(int? limit = null)
		{
			var serviceObject = RetrieveApiData(null, WebConfigurationManager.AppSettings["NewsBlogShortname"], "news", limit);
			return serviceObject?.Response.Posts;
		}

		/// <inheritdoc cref="ITumblrClient"/>
		public IPost GetPost(string postId, string blogShortname)
		{
			if (string.IsNullOrWhiteSpace(postId))
			{
				return null;
			}
			var serviceObject = RetrieveApiData(postId, blogShortname);
			if (serviceObject?.Response?.Posts != null)
			{
				return serviceObject.Response.Posts.FirstOrDefault();
			}
			RefreshApiCache(postId, blogShortname);
			var updatedObject = RetrieveApiData(postId, blogShortname);
			return updatedObject?.Response?.Posts?.FirstOrDefault();
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

		private static ServiceObject RetrieveApiData(string postId, string blogShortname, string tag = null, int? limit = null)
		{
			var request = new RestRequest("blog/" + blogShortname + ".tumblr.com/posts", Method.GET);
			request.AddParameter("api_key", ApiKey);
			request.AddParameter("notes_info", "true");
			if (postId != null)
			{
				request.AddParameter("id", postId);
			}
			if (tag != null)
			{
				request.AddParameter("tag", tag);
			}
			if (limit != null)
			{
				request.AddParameter("limit", limit);
			}
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var response = _client.Execute<ServiceObject>(request);
			var serviceObject = response.Data;
			return serviceObject;
		}
	}
}
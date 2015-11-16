using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;
using RestSharp;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.ServiceModels;

namespace TumblrThreadTracker.Infrastructure.Services
{
    public class TumblrClient : ITumblrClient
    {
        private const string ApiKey = "***REMOVED***";
        private static IRestClient _client;

        public TumblrClient(IRestClient client)
        {
            _client = client;
        }

        public IPost GetPost(string postId, string blogShortname)
        {
            if (string.IsNullOrWhiteSpace(postId))
                return null;
            var serviceObject = RetrieveApiData(postId, blogShortname);
            if (serviceObject != null && serviceObject.response != null)
                return serviceObject.response.posts.FirstOrDefault();
            RefreshApiCache(postId, blogShortname);
            var updatedObject = RetrieveApiData(postId, blogShortname);
            if (updatedObject != null && updatedObject.response != null)
                return updatedObject.response.posts.FirstOrDefault();
            return null;
        }

        public IEnumerable<Post> GetNewsPosts(int? limit = null)
        {
            var serviceObject = RetrieveApiData(null, WebConfigurationManager.AppSettings["NewsBlogShortname"], "news",
                limit);
            return serviceObject != null ? serviceObject.response.posts : null;
        }

        private static ServiceObject RetrieveApiData(string postId, string blogShortname, string tag = null,
            int? limit = null)
        {
            var request = new RestRequest("blog/" + blogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("notes_info", "true");
            if (postId != null)
                request.AddParameter("id", postId);
            if (tag != null)
                request.AddParameter("tag", tag);
            if (limit != null)
                request.AddParameter("limit", limit);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = _client.Execute<ServiceObject>(request);
            var serviceObject = response.Data;
            return serviceObject;
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
                //honestly I don't care if it 404ed
            }
        }
    }
}
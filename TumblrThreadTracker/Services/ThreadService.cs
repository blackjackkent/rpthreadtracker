using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using RestSharp;
using TumblrThreadTracker.Models.Service_Models;

namespace TumblrThreadTracker.Services
{
    public class ThreadService
    {
        private const string api_key = "***REMOVED***";
        private static readonly RestClient _client = new RestClient("http://api.tumblr.com/v2");

        public static Post GetPost(string postId, string blogShortname)
        {
            ServiceObject serviceObject = RetrieveApiData(postId, blogShortname);
            if (serviceObject != null && serviceObject.response != null)
                return serviceObject.response.posts.FirstOrDefault();
            RefreshApiCache(postId, blogShortname);
            ServiceObject updatedObject = RetrieveApiData(postId, blogShortname);
            if (updatedObject != null && updatedObject.response != null)
                return updatedObject.response.posts.FirstOrDefault();
            return null;
        }

        public static IEnumerable<Post> GetNewsPosts(int limit)
        {
            ServiceObject serviceObject = RetrieveApiData(null, WebConfigurationManager.AppSettings["NewsBlogShortname"], "news", limit);
            if (serviceObject != null)
                return serviceObject.response.posts;
            return null;
        }

        private static ServiceObject RetrieveApiData(string postId, string blogShortname, string tag = null, int? limit = null)
        {
            var request = new RestRequest("blog/" + blogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", api_key);
            request.AddParameter("notes_info", "true");
            if (postId != null)
                request.AddParameter("id", postId);
            if (tag != null)
                request.AddParameter("tag", tag);
            if (limit != null)
                request.AddParameter("limit", limit);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse<ServiceObject> response = _client.Execute<ServiceObject>(request);
            ServiceObject serviceObject = response.Data;
            return serviceObject;
        }

        private static void RefreshApiCache(string postId, string blogShortname)
        {
            string sURL;
            sURL = "http://" + blogShortname + ".tumblr.com/post/" + postId;
            WebRequest wrGETURL = WebRequest.Create(sURL);
            try
            {
                wrGETURL.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                //honestly I don't care if it 404ed
            }
        }
    }
}
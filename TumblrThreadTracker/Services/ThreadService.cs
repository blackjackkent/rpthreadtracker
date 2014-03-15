using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using TumblrThreadTracker.Factories;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.Service_Models;
using TumblrThreadTracker.Models.ViewModels;

namespace TumblrThreadTracker.Services
{
    public class ThreadService
    {
        private const string api_key = "***REMOVED***";
        private static readonly RestClient _client = new RestClient("http://api.tumblr.com/v2");

        public ThreadService()
        {
        }

        public static Thread GetThread(string postId, string blogShortname, string userTitle) {
            ServiceResponse serviceResponse = new ServiceResponse();
            Thread thread = new Thread();

            var request = new RestRequest("blog/" + blogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", api_key);
            request.AddParameter("id", postId);
            request.AddParameter("notes_info", "true");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse<ServiceObject> response = _client.Execute<ServiceObject>(request);
            ServiceObject serviceObject = response.Data;
            if (serviceObject != null)
            {
                thread = ThreadFactory.BuildFromService(serviceObject.response, userTitle, blogShortname);
            }
            return thread;
        }

    }
}
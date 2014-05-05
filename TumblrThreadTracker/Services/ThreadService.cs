using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Web.Services.Description;
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
        private static string _newsBlogShortname = "tblrthreadtracker";

        public ThreadService()
        {
        }

        public static Thread GetThread(string postId, string blogShortname, string userTitle) {
            ServiceResponse serviceResponse = new ServiceResponse();
            Thread thread;
            ServiceObject serviceObject = RetrieveApiData(postId, blogShortname);
            if (serviceObject != null)
            {
                thread = ThreadFactory.BuildFromService(serviceObject.response, userTitle, blogShortname, postId);
                return thread;
            }
            SendGetRequestToPost(postId, blogShortname);
            ServiceObject updatedObject = RetrieveApiData(postId, blogShortname);
            if (updatedObject != null)
            {
                return ThreadFactory.BuildFromService(serviceObject.response, userTitle, blogShortname, postId);
            }
            return ThreadFactory.BuildFromService(null, userTitle, blogShortname, postId);
        }

        public static ServiceObject RetrieveApiData(string postId, string blogShortname)
        {
            var request = new RestRequest("blog/" + blogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", api_key);
            request.AddParameter("id", postId);
            request.AddParameter("notes_info", "true");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse<ServiceObject> response = _client.Execute<ServiceObject>(request);
            ServiceObject serviceObject = response.Data;
            return serviceObject;
        }

        private static void SendGetRequestToPost(string postId, string blogShortname)
        {
            string sURL;
            sURL = "http://" + blogShortname + ".tumblr.com/post/" + postId;
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            Stream objStream;
            try
            {
                objStream = wrGETURL.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                //honestly I don't care if it 404ed
            }
        }

        public static Thread GetNewsThread()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Thread thread = new Thread();
            var request = new RestRequest("blog/" + _newsBlogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", api_key);
            request.AddParameter("tag", "news");
            request.AddParameter("limit", 1);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse<ServiceObject> response = _client.Execute<ServiceObject>(request);
            ServiceObject serviceObject = response.Data;
            if (serviceObject != null)
            {
                thread = ThreadFactory.BuildFromService(serviceObject.response, serviceObject.response.posts[0].title, _newsBlogShortname, serviceObject.response.posts[0].id.ToString());
            }
            else
            {
                thread = ThreadFactory.BuildFromService(null, null, _newsBlogShortname, null);
            }
            return thread;
        }

    }
}
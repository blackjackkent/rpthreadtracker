using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Services.Description;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using TumblrThreadTracker.Domain.Threads;
using TumblrThreadTracker.Models.Service_Models;

namespace TumblrThreadTracker.Services
{
    public class ThreadService
    {
        private const string api_key = "***REMOVED***";
        private static readonly RestClient _client = new RestClient("http://api.tumblr.com/v2");
        private static string _newsBlogShortname = "tblrthreadtracker";

        public static Post GetPost(string postId, string blogShortname) {
            ServiceObject serviceObject = RetrieveApiData(postId, blogShortname);
            if (serviceObject != null && serviceObject.response != null)
                return serviceObject.response.posts.FirstOrDefault();
            RefreshApiCache(postId, blogShortname);
            ServiceObject updatedObject = RetrieveApiData(postId, blogShortname);
            if (updatedObject != null && updatedObject.response != null)
                return updatedObject.response.posts.FirstOrDefault();
            return null;
        }

        private static ServiceObject RetrieveApiData(string postId, string blogShortname)
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

        private static void RefreshApiCache(string postId, string blogShortname)
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

        public static IEnumerable<Thread> GetNewsThreads(int limit)
        {
            /*List<Thread> threads;
            var request = new RestRequest("blog/" + _newsBlogShortname + ".tumblr.com/posts", Method.GET);
            request.AddParameter("api_key", api_key);
            request.AddParameter("tag", "news");
            request.AddParameter("limit", limit);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            IRestResponse<ServiceObject> response = _client.Execute<ServiceObject>(request);
            ServiceObject serviceObject = response.Data;
            if (serviceObject != null)
            {
                foreach (var post in serviceObject.response.posts)
                {
                    threads.Add(new Thread(post));
                }
                thread = ThreadFactory.BuildFromService(serviceObject.response, serviceObject.response.posts[0].title, _newsBlogShortname, serviceObject.response.posts[0].id.ToString());
            }
            else
            {
                thread = ThreadFactory.BuildFromService(null, null, _newsBlogShortname, null);
            }
            return thread;*/
            throw new NotImplementedException();
        }

    }
}
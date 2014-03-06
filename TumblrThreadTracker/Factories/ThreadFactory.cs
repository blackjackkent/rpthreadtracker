using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DataModels;
using TumblrThreadTracker.Models.Service_Models;
using TumblrThreadTracker.Models.ViewModels;

namespace TumblrThreadTracker.Factories
{
    public class ThreadFactory
    {
        public static Thread BuildFromService(ServiceResponse serviceResponse, string userTitle)
        {
            if (serviceResponse == null)
            {
                return new Thread();
            }

            Post post = serviceResponse.posts.FirstOrDefault();
            List<Thread> threads = new List<Thread>();

            long postId = post.id;
            string type = post.type;
            string blogShortname = serviceResponse.blog.name;
            Note mostRecentNote = null;
            string lastPosterShortname = null;
            string lastPostUrl = null;
            if (post.notes != null && post.notes.Where(n => n.type == "reblog").Any())
            {
                mostRecentNote = post.notes.OrderByDescending(n => n.timestamp).Where(n => n.type == "reblog").FirstOrDefault();
                lastPosterShortname = mostRecentNote.blog_name;
                lastPostUrl = mostRecentNote.blog_url + "post/" + mostRecentNote.post_id;
            }
            else
            {
                lastPosterShortname = post.blog_name;
                lastPostUrl = post.post_url;
            }
            return new Thread
            {
                PostId = postId,
                UserTitle = userTitle,
                Type = type,
                BlogShortname = blogShortname,
                LastPosterShortname = lastPosterShortname,
                LastPostUrl = lastPostUrl
            };
        }

        public static UserThread BuildDataModel(string postId, int userBlogId, string userTitle)
        {
            UserThread thread = new UserThread
            {
                PostId = postId,
                UserBlogId = userBlogId,
                UserTitle = userTitle
            };
            return thread;
        }
    
    }
}
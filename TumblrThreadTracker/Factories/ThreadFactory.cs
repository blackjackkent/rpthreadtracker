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
        public static Thread BuildFromService(ServiceResponse serviceResponse, string userTitle, string userBlogShortname)
        {
            if (serviceResponse == null)
            {
                return null;
            }

            Post post = serviceResponse.posts.FirstOrDefault();

            if (post == null)
            {
                return new Thread();
            }
            long postId = post.id;
            string type = post.type;
            string blogShortname = serviceResponse.blog.name;
            string lastPosterShortname = null;
            string lastPostUrl = null;
            if (post.notes != null && post.notes.Any(n => n.type == "reblog"))
            {
                Note mostRecentNote = post.notes.OrderByDescending(n => n.timestamp).FirstOrDefault(n => n.type == "reblog");
                if (mostRecentNote != null)
                {
                    lastPosterShortname = mostRecentNote.blog_name;
                    lastPostUrl = mostRecentNote.blog_url + "post/" + mostRecentNote.post_id;
                }
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
                LastPostUrl = lastPostUrl,
                IsMyTurn = lastPosterShortname != userBlogShortname
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
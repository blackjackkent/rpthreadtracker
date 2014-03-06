using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataModels = TumblrThreadTracker.Models.DataModels;
using ViewModels = TumblrThreadTracker.Models.ViewModels;

namespace TumblrThreadTracker.Factories
{
    public class BlogFactory
    {
        public static DataModels.UserBlog BuildFromViewModel(ViewModels.UserBlog viewBlog, int userId)
        {
            DataModels.UserBlog dataBlog = DataModels.UserBlog.Create(viewBlog.BlogShortname, userId);
            return dataBlog;
        }

        public static IEnumerable<ViewModels.UserBlog> BuildFromDataModel(IEnumerable<DataModels.UserBlog> dataBlogs)
        {
            List<ViewModels.UserBlog> viewBlogs = new List<ViewModels.UserBlog>();
            foreach (DataModels.UserBlog dataBlog in dataBlogs)
            {
                ViewModels.UserBlog viewBlog = BuildFromDataModel(dataBlog);
                viewBlogs.Add(viewBlog);
            }
            return viewBlogs;
        }

        public static ViewModels.UserBlog BuildFromDataModel(DataModels.UserBlog dataBlog)
        {
            ViewModels.UserBlog viewBlog = new ViewModels.UserBlog
            {
                UserId = dataBlog.UserId,
                UserBlogId = dataBlog.UserBlogId,
                BlogShortname = dataBlog.BlogShortname,
                Threads = new List<ViewModels.Thread>()
            };
            return viewBlog;
        }
    }
}
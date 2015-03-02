using System.Collections.Generic;
using TumblrThreadTracker.Models.ServiceModels;

namespace TumblrThreadTracker.Interfaces
{
    public interface ITumblrClient
    {
        IPost GetPost(string postId, string blogShortname);
        IEnumerable<Post> GetNewsPosts(int? count = null);
    }
}
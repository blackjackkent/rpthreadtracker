using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumblrThreadTracker.Models.Service_Models;

namespace TumblrThreadTracker.Interfaces
{
    public interface IPost
    {
        string blog_name { get; set; }
        long id { get; set; }
        string post_url { get; set; }
        string type { get; set; }
        long timestamp { get; set; }
        string date { get; set; }
        string format { get; set; }
        string reblog_key { get; set; }
        List<string> tags { get; set; }
        bool bookmarklet { get; set; }
        bool mobile { get; set; }
        string source_url { get; set; }
        string source_title { get; set; }
        string title { get; set; }
        bool liked { get; set; }
        string state { get; set; }
        long total_posts { get; set; }
        List<Note> notes { get; set; }

        Note GetMostRecentRelevantNote(string blogShortname, string watchedShortname);
    }
}

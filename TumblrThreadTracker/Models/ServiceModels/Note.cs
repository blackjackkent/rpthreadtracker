namespace TumblrThreadTracker.Models.ServiceModels
{
	public class Note
	{
		public string added_text { get; set; }

		public string blog_name { get; set; }

		public string blog_url { get; set; }

		public string post_id { get; set; }

		public long timestamp { get; set; }

		public string type { get; set; }
	}
}
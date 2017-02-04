namespace TumblrThreadTracker.Models.RequestModels
{
	public class LoginRequest
	{
		public string Password { get; set; }

		public bool RememberMe { get; set; }

		public string UserName { get; set; }
	}
}
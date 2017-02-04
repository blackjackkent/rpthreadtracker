namespace TumblrThreadTracker.Models.RequestModels
{
	public class RegisterRequest
	{
		public string ConfirmPassword { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Username { get; set; }
	}
}
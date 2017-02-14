namespace RPThreadTracker.Models.RequestModels
{
	/// <summary>
	/// Request body transmitted during signup process
	/// </summary>
	public class RegisterRequest
	{
		/// <summary>
		/// Gets or sets the user's confirmation of password for new account
		/// </summary>
		/// <value>
		/// String confirming the user's requested password
		/// </value>
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets email address to be associated with new account
		/// </summary>
		/// <value>
		/// String containing email address for account
		/// </value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's requested password
		/// </summary>
		/// <value>
		/// String representing the user's requested password
		/// </value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets username to be associated with new account
		/// </summary>
		/// <value>
		/// String containing username for account
		/// </value>
		public string Username { get; set; }
	}
}
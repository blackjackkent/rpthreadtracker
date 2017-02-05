namespace TumblrThreadTracker.Models.DomainModels.Account
{
	/// <summary>
	/// Domain Model class representing user authentication info
	/// </summary>
	public class WebpagesMembership : DomainModel
	{
		/// <summary>
		/// Gets or sets the token value used to verify a password reset
		/// </summary>
		/// <value>
		/// String value of token
		/// </value>
		public string PasswordVerificationToken { get; set; }

		/// <summary>
		/// Gets or sets unique identifier of user profile associated with membership info
		/// </summary>
		/// <value>
		/// Integer value of user profile ID
		/// </value>
		public int UserId { get; set; }
	}
}
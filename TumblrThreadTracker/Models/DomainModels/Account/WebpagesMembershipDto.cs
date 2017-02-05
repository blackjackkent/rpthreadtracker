namespace TumblrThreadTracker.Models.DomainModels.Account
{
	using Interfaces;

	/// <summary>
	/// DTO object for transferring <see cref="WebpagesMembership" /> data
	/// </summary>
	public class WebpagesMembershipDto : IDto<WebpagesMembership>
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

		/// <inheritdoc cref="IDto{TModel}"/>
		public WebpagesMembership ToModel()
		{
			return new WebpagesMembership
			{
				UserId = UserId,
				PasswordVerificationToken = PasswordVerificationToken,
			};
		}
	}
}
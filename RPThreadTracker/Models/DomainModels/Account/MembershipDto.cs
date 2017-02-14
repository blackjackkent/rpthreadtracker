namespace RPThreadTracker.Models.DomainModels.Account
{
	using Interfaces;

	/// <summary>
	/// DTO object for transferring <see cref="Membership" /> data
	/// </summary>
	public class MembershipDto : IDto<Membership>
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
		public Membership ToModel()
		{
			return new Membership
			{
				UserId = UserId,
				PasswordVerificationToken = PasswordVerificationToken,
			};
		}
	}
}
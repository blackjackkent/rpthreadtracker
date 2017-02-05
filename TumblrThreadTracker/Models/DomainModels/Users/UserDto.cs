namespace TumblrThreadTracker.Models.DomainModels.Users
{
	using Interfaces;

	/// <summary>
	/// DTO object for transferring <see cref="User" /> data
	/// </summary>
	public class UserDto : IDto<User>
	{
		/// <summary>
		/// Gets or sets the email address associated with this account
		/// </summary>
		/// <value>
		/// String value of email address
		/// </value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user wants to display their thread count on the dashboard.
		/// </summary>
		/// <value>
		/// True if distribution should be displayed, false if not.
		/// </value>
		public bool ShowDashboardThreadDistribution { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user wants to use an inverted CSS theme
		/// </summary>
		/// <value>
		/// True if inverted theme should be used, false if not.
		/// </value>
		public bool UseInvertedTheme { get; set; }

		/// <summary>
		/// Gets or sets the user's unique identifier in the tracker database
		/// </summary>
		/// <value>
		/// Integer identifier for the user
		/// </value>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the username for the user's account
		/// </summary>
		/// <value>
		/// String value of the username
		/// </value>
		public string UserName { get; set; }

		/// <inheritdoc cref="IDto{TModel}"/>
		public User ToModel()
		{
			return new User(this);
		}
	}
}
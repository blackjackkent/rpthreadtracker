namespace RPThreadTracker.Models.DomainModels.Users
{
	/// <summary>
	/// Domain Model class representing a user account
	/// </summary>
	public class User : DomainModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class
		/// </summary>
		public User()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User"/> class
		/// </summary>
		/// <param name="dto"><see cref="UserDto"/> object to convert to domain model</param>
		public User(UserDto dto)
		{
			UserId = dto.UserId;
			UserName = dto.UserName;
			Email = dto.Email;
			ShowDashboardThreadDistribution = dto.ShowDashboardThreadDistribution;
			UseInvertedTheme = dto.UseInvertedTheme;
		}

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

		/// <summary>
		/// Converts <see cref="User"/> object to <see cref="UserDto"/>
		/// </summary>
		/// <returns><see cref="UserDto"/> object corresponding to this user</returns>
		public UserDto ToDto()
		{
			return new UserDto
			{
				UserId = UserId,
				Email = Email,
				UserName = UserName,
				ShowDashboardThreadDistribution = ShowDashboardThreadDistribution,
				UseInvertedTheme = UseInvertedTheme
			};
		}
	}
}
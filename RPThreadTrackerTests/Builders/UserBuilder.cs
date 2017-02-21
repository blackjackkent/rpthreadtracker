namespace RPThreadTrackerTests.Builders
{
	using RPThreadTracker.Models.DomainModels.Users;

	internal class UserBuilder
	{
		private int _userId = 1;
		private string _email = "test@test.com";
		private bool _showDashboardThreadDistribution = true;
		private bool _useInvertedTheme = false;
		private string _username = "testaccount";

		public User Build()
		{
			return new User
			{
				UserId = _userId,
				Email = _email,
				ShowDashboardThreadDistribution = _showDashboardThreadDistribution,
				UseInvertedTheme = _useInvertedTheme,
				UserName = _username
			};
		}

		public UserDto BuildDto()
		{
			return new UserDto
			{
				UserId = _userId,
				Email = _email,
				ShowDashboardThreadDistribution = _showDashboardThreadDistribution,
				UseInvertedTheme = _useInvertedTheme,
				UserName = _username
			};
		}

		public UserBuilder WithUserId(int userId)
		{
			_userId = userId;
			return this;
		}

		public UserBuilder WithEmail(string email)
		{
			_email = email;
			return this;
		}

		public UserBuilder WithShowDashboardThreadDistribution(bool showDashboardThreadDistribution)
		{
			_showDashboardThreadDistribution = showDashboardThreadDistribution;
			return this;
		}

		public UserBuilder WithUseInvertedTheme(bool useInvertedTheme)
		{
			_useInvertedTheme = useInvertedTheme;
			return this;
		}

		public UserBuilder WithUsername(string username)
		{
			_username = username;
			return this;
		}
	}
}

namespace TumblrThreadTracker.Models.DomainModels.Users
{
	using System;

	using Interfaces;

	public class UserDto : IDto<User>
	{
		public string Email { get; set; }

		public DateTime? LastLogin { get; set; }

		public bool ShowDashboardThreadDistribution { get; set; }

		public bool UseInvertedTheme { get; set; }

		public int UserId { get; set; }

		public string UserName { get; set; }

		public User ToModel()
		{
			return new User(this);
		}
	}
}
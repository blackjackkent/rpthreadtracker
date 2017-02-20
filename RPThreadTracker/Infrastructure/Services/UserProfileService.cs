namespace RPThreadTracker.Infrastructure.Services
{
	using System.Linq;
	using Interfaces;
	using Models.DomainModels.Users;

	/// <inheritdoc cref="IUserProfileService"/>
	public class UserProfileService : IUserProfileService
	{
		/// <inheritdoc cref="IUserProfileService"/>
		public void Update(UserDto user, IRepository<User> userProfileRepository)
		{
			userProfileRepository.Update(user.UserId, user.ToModel());
		}

		/// <inheritdoc cref="IUserProfileService"/>
		public bool UserExistsWithUsername(string username, IRepository<User> userProfileRepository)
		{
			var user = GetUserByUsername(username, userProfileRepository);
			return user != null;
		}

		/// <inheritdoc cref="IUserProfileService"/>
		public bool UserExistsWithEmail(string email, IRepository<User> userProfileRepository)
		{
			var user = GetUserByEmail(email, userProfileRepository);
			return user != null;
		}

		/// <inheritdoc cref="IUserProfileService"/>
		public UserDto GetUserByUsername(string username, IRepository<User> userProfileRepository)
		{
			var user = userProfileRepository.GetSingle(u => u.UserName == username);
			return user?.ToDto();
		}

		/// <inheritdoc cref="IUserProfileService"/>
		public UserDto GetUserByEmail(string email, IRepository<User> userProfileRepository)
		{
			var user = userProfileRepository.GetSingle(u => u.Email == email);
			return user?.ToDto();
		}
	}
}
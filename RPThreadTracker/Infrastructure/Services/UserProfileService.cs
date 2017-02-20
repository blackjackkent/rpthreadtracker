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
			return userProfileRepository.Get(u => u.UserName == username).Any();
		}

		/// <inheritdoc cref="IUserProfileService"/>
		public bool UserExistsWithEmail(string email, IRepository<User> userProfileRepository)
		{
			return userProfileRepository.Get(u => u.Email == email).Any();
		}
	}
}
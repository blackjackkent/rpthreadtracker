namespace TumblrThreadTracker.Infrastructure.Services
{
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
	}
}
namespace TumblrThreadTracker.Infrastructure.Services
{
	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Users;

	public class UserProfileService : IUserProfileService
	{
		public void Update(UserDto user, IRepository<User> userProfileRepository)
		{
			userProfileRepository.Update(user.UserId, user.ToModel());
		}
	}
}
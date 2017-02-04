namespace TumblrThreadTracker.Interfaces
{
	using TumblrThreadTracker.Models.DomainModels.Users;

	public interface IUserProfileService
	{
		void Update(UserDto user, IRepository<User> userProfileRepository);
	}
}
namespace TumblrThreadTracker.Interfaces
{
	using Models.DomainModels.Users;

	/// <summary>
	/// Class which facilitates interaction with repository layer
	/// for retrieving <see cref="User"/> data
	/// </summary>
	public interface IUserProfileService
	{
		/// <summary>
		/// Updates existing user with passed property information
		/// </summary>
		/// <param name="dto"><see cref="UserDto"/> object containing data to be updated on database object</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		void Update(UserDto dto, IRepository<User> userProfileRepository);
	}
}
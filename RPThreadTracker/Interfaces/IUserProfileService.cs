namespace RPThreadTracker.Interfaces
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

		/// <summary>
		/// Returns the user from the database which has a passed username
		/// </summary>
		/// <param name="username">Username for the user to retrieve</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns><see cref="UserDto"/> object representing user to which the username belongs, or null if not found</returns>
		UserDto GetUserByUsername(string username, IRepository<User> userProfileRepository);

		/// <summary>
		/// Returns the user from the database which has a passed email address
		/// </summary>
		/// <param name="email">Email for the user to retrieve</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns><see cref="UserDto"/> object representing user to which the username belongs, or null if not found</returns>
		UserDto GetUserByEmail(string email, IRepository<User> userProfileRepository);
	}
}
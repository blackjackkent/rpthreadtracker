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
		/// Determines whether or not a user exists with a particular username
		/// </summary>
		/// <param name="username">Username string to verify</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns>True if account exists with passed username, false if not</returns>
		bool UserExistsWithUsername(string username, IRepository<User> userProfileRepository);

		/// <summary>
		/// Determines whether or not a user exists with a particular email address
		/// </summary>
		/// <param name="email">Email string to verify</param>
		/// <param name="userProfileRepository">Repository object containing database connection</param>
		/// <returns>True if account exists with passed email, false if not</returns>
		bool UserExistsWithEmail(string email, IRepository<User> userProfileRepository);
	}
}
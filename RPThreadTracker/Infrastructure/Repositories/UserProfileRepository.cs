namespace RPThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;
	using Interfaces;
	using Models.DomainModels.Users;

	/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
	public class UserProfileRepository : BaseRepository<User, UserProfile>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserProfileRepository"/> class
		/// </summary>
		/// <param name="context">Entity framework data context to use for tracker data management</param>
		public UserProfileRepository(IThreadTrackerContext context)
		{
			Context = context;
			DbSet = context.UserProfiles;
		}

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IThreadTrackerContext Context { get; }

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IDbSet<UserProfile> DbSet { get; }
	}
}
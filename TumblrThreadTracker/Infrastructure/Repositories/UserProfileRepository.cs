namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;

	using Interfaces;
	using Models.DomainModels.Users;

	public class UserProfileRepository : BaseRepository<User, UserProfile>
	{
		private readonly IThreadTrackerContext _context;

		private readonly IDbSet<UserProfile> _dbSet;

		public UserProfileRepository(IThreadTrackerContext context)
		{
			_context = context;
			_dbSet = context.UserProfiles;
		}

		protected override IThreadTrackerContext Context
		{
			get
			{
				return _context;
			}
		}

		protected override IDbSet<UserProfile> DbSet
		{
			get
			{
				return _dbSet;
			}
		}
	}
}
namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using Interfaces;
	using Models.DomainModels.Threads;

	/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
	public class UserThreadRepository : BaseRepository<Thread, UserThread>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserThreadRepository"/> class
		/// </summary>
		/// <param name="context">Entity framework data context to use for tracker data management</param>
		public UserThreadRepository(IThreadTrackerContext context)
		{
			Context = context;
			DbSet = context.UserThreads;
		}

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IThreadTrackerContext Context { get; }

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IDbSet<UserThread> DbSet { get; }
	}
}
namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;
	using Interfaces;
	using Models.DomainModels.Account;

	/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
	public class MembershipRepository : BaseRepository<Membership, WebpagesMembership>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MembershipRepository"/> class
		/// </summary>
		/// <param name="context">Entity framework data context to use for tracker data management</param>
		public MembershipRepository(IThreadTrackerContext context)
		{
			Context = context;
			DbSet = context.WebpagesMemberships;
		}

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IThreadTrackerContext Context { get; }

		/// <inheritdoc cref="BaseRepository{TModel,TEntity}"/>
		protected override IDbSet<WebpagesMembership> DbSet { get; }
	}
}
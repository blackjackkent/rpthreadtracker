namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;

	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Account;

	public class WebpagesMembershipRepository :
		BaseRepository<WebpagesMembership, webpages_Membership>
	{
		private readonly IThreadTrackerContext _context;

		private readonly IDbSet<webpages_Membership> _dbSet;

		public WebpagesMembershipRepository(IThreadTrackerContext context)
		{
			_context = context;
			_dbSet = context.webpages_Membership;
		}

		protected override IThreadTrackerContext Context
		{
			get
			{
				return _context;
			}
		}

		protected override IDbSet<webpages_Membership> DbSet
		{
			get
			{
				return _dbSet;
			}
		}
	}
}
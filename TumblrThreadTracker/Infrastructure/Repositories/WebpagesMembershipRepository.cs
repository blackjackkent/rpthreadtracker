using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class WebpagesMembershipRepository : BaseRepository<WebpagesMembership, WebpagesMembershipDto, webpages_Membership>
    {
        private readonly IThreadTrackerContext _context;
        private readonly IDbSet<webpages_Membership> _dbSet; 
        protected override IThreadTrackerContext Context
        {
            get { return _context; }
        }

        protected override IDbSet<webpages_Membership> DbSet
        {
            get { return _dbSet; }
        }

        public WebpagesMembershipRepository(IThreadTrackerContext context)
        {
            _context = context;
            _dbSet = context.webpages_Membership;
        }
    }
}
using System.Data.Entity;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Users;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository<User, UserDto, UserProfile>
    {
        private readonly IThreadTrackerContext _context;
        private readonly IDbSet<UserProfile> _dbSet; 
        protected override IThreadTrackerContext Context
        {
            get { return _context; }
        }

        protected override IDbSet<UserProfile> DbSet
        {
            get { return _dbSet; }
        }

        public UserProfileRepository(IThreadTrackerContext context)
        {
            _context = context;
            _dbSet = context.UserProfiles;
        }
    }
}
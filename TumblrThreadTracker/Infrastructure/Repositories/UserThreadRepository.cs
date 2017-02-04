namespace TumblrThreadTracker.Infrastructure.Repositories
{
	using System.Data.Entity;
	using System.Linq;

	using TumblrThreadTracker.Interfaces;
	using TumblrThreadTracker.Models.DomainModels.Threads;

	public class UserThreadRepository : BaseRepository<Thread, UserThread>
	{
		private readonly IThreadTrackerContext _context;

		private readonly IDbSet<UserThread> _dbSet;

		public UserThreadRepository(IThreadTrackerContext context)
		{
			_context = context;
			_dbSet = context.UserThreads;
		}

		protected override IThreadTrackerContext Context
		{
			get
			{
				return _context;
			}
		}

		protected override IDbSet<UserThread> DbSet
		{
			get
			{
				return _dbSet;
			}
		}

		public override void Delete(object id)
		{
			var threadEntity = _dbSet.Find(id);
			threadEntity.UserThreadTags.ToList().ForEach(t => threadEntity.UserThreadTags.Remove(t));
			_context.Commit();
			base.Delete(id);
		}

		public override Thread Insert(Thread model)
		{
			var insertedModel = base.Insert(model);
			var threadEntity = _dbSet.Find(insertedModel.UserThreadId);
			foreach (var updatedTag in model.ThreadTags)
			{
				threadEntity.UserThreadTags.Add(new UserThreadTag { TagText = updatedTag });
			}

			_context.Commit();
			return threadEntity.ToModel<Thread, UserThread>();
		}

		public override Thread Update(object id, Thread model)
		{
			base.Update(id, model);
			if (model.ThreadTags != null)
			{
				var threadEntity = _dbSet.Find(model.UserThreadId);
				var existingTags = threadEntity.UserThreadTags.ToList();
				foreach (var existingTag in existingTags)
				{
					var tagHasNotBeenDeleted = model.ThreadTags.Any(i => i == existingTag.TagText);

					if (!tagHasNotBeenDeleted) _context.UserThreadTags.Remove(existingTag);
				}

				foreach (var updatedTag in model.ThreadTags)
				{
					if (!existingTags.Any(i => i.TagText == updatedTag)) threadEntity.UserThreadTags.Add(new UserThreadTag { TagText = updatedTag });
				}
			}

			_context.Commit();
			return GetSingle(t => t.UserThreadId == model.UserThreadId);
		}
	}
}
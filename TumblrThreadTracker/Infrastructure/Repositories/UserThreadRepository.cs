using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Threads;
using WebGrease.Css.Extensions;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class UserThreadRepository : BaseRepository<Thread, ThreadDto, UserThread>
    {
        private readonly IThreadTrackerContext _context;
        private readonly IDbSet<UserThread> _dbSet; 
        protected override IThreadTrackerContext Context
        {
            get { return _context; }
        }

        protected override IDbSet<UserThread> DbSet
        {
            get { return _dbSet; }
        }

        public override Thread Insert(Thread model)
        {
            var insertedModel = base.Insert(model);
            var threadEntity = _dbSet.Find(insertedModel.UserThreadId);
            foreach (var updatedTag in model.ThreadTags)
            {
                threadEntity.UserThreadTags.Add(new UserThreadTag
                {
                    TagText = updatedTag
                });
            }
            _context.Commit();
            return threadEntity.ToModel<Thread, UserThread>();
        }

        public override Thread Update(object id, Thread model)
        {
            base.Update(id, model);
            var threadEntity = _dbSet.Find(model.UserThreadId);
            var existingTags = threadEntity.UserThreadTags.ToList();
            foreach (var existingTag in existingTags)
            {
                var tagHasNotBeenDeleted = model.ThreadTags.Any(i => i == existingTag.TagText);

                if (!tagHasNotBeenDeleted)
                    _context.UserThreadTags.Remove(existingTag);
            }

            foreach (var updatedTag in model.ThreadTags)
            {
                if (!existingTags.Any(i => i.TagText == updatedTag))
                    threadEntity.UserThreadTags.Add(new UserThreadTag
                    {
                        TagText = updatedTag
                    });
            }
            _context.Commit();
            return GetSingle(t => t.UserThreadId == model.UserThreadId);
        }

        public override void Delete(object id)
        {
            var threadEntity = _dbSet.Find(id);
            threadEntity.UserThreadTags.ToList().ForEach(t => threadEntity.UserThreadTags.Remove(t));
            _context.Commit();
            base.Delete(id);
        }

        public UserThreadRepository(IThreadTrackerContext context)
        {
            _context = context;
            _dbSet = context.UserThreads;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Infrastructure.Entities;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public abstract class BaseRepository<TModel, TDto, TEntity> :
        IRepository<TModel>
        where TModel : DomainModel
        where TDto : IDto<TModel>
        where TEntity : class, IEntity, new()
    {
        protected abstract IThreadTrackerContext Context { get; }
        protected abstract IDbSet<TEntity> DbSet { get; }

        public TModel GetSingle(Expression<Func<TModel, bool>> filter)
        {
            return DbSet.ToSelection<TModel, TEntity>().FirstOrDefault(filter);
        }

        public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            return DbSet.ToSelection<TModel, TEntity>().Where(filter);
        }

        public void Insert(TModel model)
        {
            var entity = DbSet.Add(model.ToEntity<TModel, TEntity>());
            Context.Commit();
        }

        public TModel Update(object id, TModel model)
        {
            var entityToUpdate = DbSet.Find(id);
            entityToUpdate.Load(model);
            Context.Commit();
            return model;
        }

        public void Delete(object id)
        {
            var entityToRemove = DbSet.Find(id);
            if (entityToRemove == null)
                throw new Exception("Not found");
            DbSet.Remove(entityToRemove);
            Context.Commit();
        }
    }
}
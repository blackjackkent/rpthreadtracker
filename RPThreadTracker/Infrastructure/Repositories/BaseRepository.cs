namespace RPThreadTracker.Infrastructure.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;
	using Interfaces;
	using Models.DomainModels;

	/// <summary>
	/// Parent class for all data access repositories
	/// </summary>
	/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
	/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
	public abstract class BaseRepository<TModel, TEntity> : IRepository<TModel>
		where TModel : DomainModel
		where TEntity : class, IEntity, new()
	{
		/// <summary>
		/// Gets EntityFramework database context customized with DbSets
		/// </summary>
		/// <value>
		/// <see cref="IThreadTrackerContext"/> object
		/// </value>
		protected abstract IThreadTrackerContext Context { get; }

		/// <summary>
		/// Gets EntityFramework DbSet matching the data layer entity
		/// </summary>
		/// <value>
		/// <see cref="IDbSet{TEntity}"/> representing the DbSet to be updated
		/// </value>
		protected abstract IDbSet<TEntity> DbSet { get; }

		/// <inheritdoc cref="IRepository{TModel}"/>
		public virtual void Delete(object id)
		{
			var entityToRemove = DbSet.Find(id);
			if (entityToRemove == null)
			{
				throw new Exception("Not found");
			}
			DbSet.Remove(entityToRemove);
			Context.Commit();
		}

		/// <inheritdoc cref="IRepository{TModel}"/>
		public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter)
		{
			return DbSet.ToSelection<TModel, TEntity>().Where(filter);
		}

		/// <inheritdoc cref="IRepository{TModel}"/>
		public TModel GetSingle(Expression<Func<TModel, bool>> filter)
		{
			return DbSet.ToSelection<TModel, TEntity>().FirstOrDefault(filter);
		}

		/// <inheritdoc cref="IRepository{TModel}"/>
		public virtual TModel Insert(TModel model)
		{
			var entity = DbSet.Add(model.ToEntity<TModel, TEntity>());
			Context.Commit();
			return entity.ToModel<TModel, TEntity>();
		}

		/// <inheritdoc cref="IRepository{TModel}"/>
		public virtual TModel Update(object id, TModel model)
		{
			var entityToUpdate = DbSet.Find(id);
			entityToUpdate.Load(model);
			Context.Commit();
			return model;
		}
	}
}
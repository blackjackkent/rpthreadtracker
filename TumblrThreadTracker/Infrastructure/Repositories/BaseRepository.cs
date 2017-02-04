namespace TumblrThreadTracker.Infrastructure.Repositories
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

		/// <summary>
		/// Delete entity with provided ID from database
		/// </summary>
		/// <param name="id">Unique identifier for object to be deleted</param>
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

		/// <summary>
		/// Get list of <see cref="TModel" /> objects based on search filter
		/// </summary>
		/// <param name="filter">Search filter expression used to search <see cref="DbSet" /></param>
		/// <returns>IEnumerable list of <see cref="TModel" /> objects</returns>
		public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter)
		{
			return DbSet.ToSelection<TModel, TEntity>().Where(filter);
		}

		/// <summary>
		/// Gets single instance of <see cref="TModel" /> class based on search filter
		/// </summary>
		/// <param name="filter">Search filter expression used to search <see cref="DbSet" /></param>
		/// <returns>First item in <see cref="DbSet" /> matching <see cref="filter" />, or null if none found</returns>
		public TModel GetSingle(Expression<Func<TModel, bool>> filter)
		{
			return DbSet.ToSelection<TModel, TEntity>().FirstOrDefault(filter);
		}

		/// <summary>
		/// Inserts a new instance of <see cref="TModel" /> into database
		/// </summary>
		/// <param name="model">Instance of <see cref="TModel" /> to be inserted</param>
		/// <returns>Inserted object after any modification during insertion</returns>
		public virtual TModel Insert(TModel model)
		{
			var entity = DbSet.Add(model.ToEntity<TModel, TEntity>());
			Context.Commit();
			return entity.ToModel<TModel, TEntity>();
		}

		/// <summary>
		/// Updates object with <see cref="id" /> based on properties of <see cref="model" />
		/// </summary>
		/// <param name="id">Unique identifier of object to be updated</param>
		/// <param name="model"><see cref="TModel" /> instance with properties to be updated.</param>
		/// <returns><see cref="TModel" /> value that has been updated</returns>
		public virtual TModel Update(object id, TModel model)
		{
			var entityToUpdate = DbSet.Find(id);
			entityToUpdate.Load(model);
			Context.Commit();
			return model;
		}
	}
}
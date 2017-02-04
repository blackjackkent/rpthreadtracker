namespace TumblrThreadTracker.Infrastructure
{
	using System;
	using System.Linq;

	using AutoMapper;
	using AutoMapper.QueryableExtensions;

	using Entities;
	using Models;
	using Models.DomainModels;

	/// <summary>
	/// Class containing extension methods for managing mapping between <see cref="Model"/> objects and <see cref="IEntity"/> objects
	/// </summary>
	public static class ModelMappingExtensionMethods
	{
		/// <summary>
		/// Loads a database-level <see cref="IEntity"/> object with mapped properties from a <see cref="TModel"/> object
		/// </summary>
		/// <typeparam name="TModel"><see cref="Model"/> object used in business layer</typeparam>
		/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
		/// <param name="entity">Calling entity to be loaded</param>
		/// <param name="model">Model containing parameters to be loaded into entity</param>
		/// <returns>Loaded entity object</returns>
		public static TEntity Load<TModel, TEntity>(this TEntity entity, TModel model) where TModel : Model
		                                                                               where TEntity : class, IEntity, new()
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}
			return Mapper.Map(model, entity);
		}

		public static TEntity ToEntity<TModel, TEntity>(this TModel model) where TModel : Model
		                                                                   where TEntity : class, IEntity, new()
		{
			var entity = new TEntity();
			return Load(entity, model);
		}

		public static TModel ToModel<TModel, TEntity>(this TEntity entity) where TModel : DomainModel where TEntity : IEntity
		{
			return Mapper.Map<TModel>(entity);
		}

		public static IQueryable<TModel> ToSelection<TModel, TEntity>(this IQueryable<TEntity> entities)
			where TModel : DomainModel where TEntity : IEntity
		{
			return entities.Project().To<TModel>();
		}
	}
}
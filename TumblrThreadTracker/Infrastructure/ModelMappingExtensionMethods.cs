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
		/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
		/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
		/// <param name="entity">Calling entity to be loaded</param>
		/// <param name="model">Model containing properties to be loaded into entity</param>
		/// <returns>Loaded entity object</returns>
		public static TEntity Load<TModel, TEntity>(this TEntity entity, TModel model)
			where TModel : DomainModel
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

		/// <summary>
		/// Maps a business layer <see cref="DomainModel"/> object to the data layer
		/// </summary>
		/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
		/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
		/// <param name="model">Model containing properties to be mapped to entity</param>
		/// <returns>Entity representation of business model</returns>
		public static TEntity ToEntity<TModel, TEntity>(this TModel model)
			where TModel : DomainModel
		    where TEntity : class, IEntity, new()
		{
			var entity = new TEntity();
			return Load(entity, model);
		}

		/// <summary>
		/// Maps a data layer <see cref="IEntity"/> object to the business layer
		/// </summary>
		/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
		/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
		/// <param name="entity">Entity object containing properties to be mapped to domain model</param>
		/// <returns>Domain layer representation of data entity</returns>
		public static TModel ToModel<TModel, TEntity>(this TEntity entity)
			where TModel : DomainModel
			where TEntity : IEntity
		{
			return Mapper.Map<TModel>(entity);
		}

		/// <summary>
		/// Maps a collection of data layer <see cref="IEntity"/> objects to the business layer
		/// </summary>
		/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
		/// <typeparam name="TEntity"><see cref="IEntity"> object used at data layer</see></typeparam>
		/// <param name="entities">Entity objects to be mapped to domain models</param>
		/// <returns>Queryable list of domain model objects</returns>
		public static IQueryable<TModel> ToSelection<TModel, TEntity>(this IQueryable<TEntity> entities)
			where TModel : DomainModel
			where TEntity : IEntity
		{
			return entities.Project().To<TModel>();
		}
	}
}
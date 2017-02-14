namespace RPThreadTracker.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Models.DomainModels;

	/// <summary>
	/// Parent interface for all data access repositories
	/// </summary>
	/// <typeparam name="TModel"><see cref="DomainModel"/> object used in business layer</typeparam>
	public interface IRepository<TModel>
	{
		/// <summary>
		/// Delete entity with provided ID from database
		/// </summary>
		/// <param name="id">Unique identifier for object to be deleted</param>
		void Delete(object id);

		/// <summary>
		/// Get list of <see cref="TModel" /> objects based on search filter
		/// </summary>
		/// <param name="filter">Search filter expression used to search database</param>
		/// <returns>IEnumerable list of <see cref="TModel" /> objects</returns>
		IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter);

		/// <summary>
		/// Gets single instance of <see cref="TModel" /> class based on search filter
		/// </summary>
		/// <param name="filter">Search filter expression used to search database</param>
		/// <returns>First item in database matching <see cref="filter" />, or null if none found</returns>
		TModel GetSingle(Expression<Func<TModel, bool>> filter);

		/// <summary>
		/// Inserts a new instance of <see cref="TModel" /> into database
		/// </summary>
		/// <param name="model">Instance of <see cref="TModel" /> to be inserted</param>
		/// <returns>Inserted object after any modification during insertion</returns>
		TModel Insert(TModel model);

		/// <summary>
		/// Updates object with <see cref="id" /> based on properties of <see cref="model" />
		/// </summary>
		/// <param name="id">Unique identifier of object to be updated</param>
		/// <param name="model"><see cref="TModel" /> instance with properties to be updated.</param>
		/// <returns><see cref="TModel" /> value that has been updated</returns>
		TModel Update(object id, TModel model);
	}
}
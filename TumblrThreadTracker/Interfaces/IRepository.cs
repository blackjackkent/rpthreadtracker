namespace TumblrThreadTracker.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public interface IRepository<TModel>
	{
		void Delete(object id);

		IEnumerable<TModel> Get(Expression<Func<TModel, bool>> criteria);

		TModel GetSingle(Expression<Func<TModel, bool>> filter);

		TModel Insert(TModel model);

		TModel Update(object id, TModel model);
	}
}
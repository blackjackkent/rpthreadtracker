using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TumblrThreadTracker.Interfaces
{
    public interface IRepository<TModel>
    {
        TModel GetSingle(Expression<Func<TModel, bool>> filter);
        IEnumerable<TModel> Get(Expression<Func<TModel, bool>> criteria);
        TModel Insert(TModel model);
        TModel Update(object id, TModel model);
        void Delete(object id);
    }
}
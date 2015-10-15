using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TumblrThreadTracker.Interfaces
{
    public interface IRepository<TModel>
    {
        TModel GetSingle(Expression<Func<TModel, bool>> filter);
        IEnumerable<TModel> Get(Expression<Func<TModel, bool>> criteria);
        void Insert(TModel entity);
        TModel Update(object id, TModel entity);
        void Delete(object id);
    }
}
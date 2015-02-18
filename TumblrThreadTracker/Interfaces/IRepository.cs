namespace TumblrThreadTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IRepository<T>
    {
        T Get(int id);
        IEnumerable<T> Get(Expression<Func<T, bool>> criteria);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int? id);
    }
}
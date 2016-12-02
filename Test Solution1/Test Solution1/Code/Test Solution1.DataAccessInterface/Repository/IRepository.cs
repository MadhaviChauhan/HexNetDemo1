using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Test_Solution1.DataAccessInterface.Repository
{
    public interface IRepository<T> : IGetAll<T>, IGetById<T>, ISave<T>, IDelete, IDisposable where T : class
    {
        T GetSingleBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}
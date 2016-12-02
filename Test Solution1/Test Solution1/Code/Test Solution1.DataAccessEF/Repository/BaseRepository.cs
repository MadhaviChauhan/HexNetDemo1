using Test_Solution1.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Test_Solution1.DataAccess.Repository
{
    public abstract class BaseRepository<TContext, TEntity> : IDisposable
        where TContext : EFContext
        where TEntity : class
    {
        private TContext _dataContext;
        private DbSet<TEntity> _dbSet;

        public BaseRepository(EFContext context)
        {
            _dataContext = (TContext)context;
            _dbSet = context.Set<TEntity>();
        }


        public TEntity GetBy(int Id)
        {
            return _dataContext.Set<TEntity>().Find(Id);
        }

        public IEnumerable<TEntity> FetchAll()
        {
            return Fetch();
        }

        protected IQueryable<TEntity> Fetch()
        {
            return _dataContext.Set<TEntity>().AsQueryable();
        }

        public TEntity GetSingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate value must be passed to FindAllBy<T>.");

            return _dataContext.Set<TEntity>().Where(predicate).SingleOrDefault();

        }

        public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate value must be passed to FindAllBy<T>.");

            return _dataContext.Set<TEntity>().Where(predicate);
        }


        public IQueryable<TEntity> GetIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dataContext.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<TEntity> StoredProc(string sprocname, params object[] sparameters)
        {
            if (sparameters != null)
            {
                return _dataContext.Database.SqlQuery<TEntity>(sprocname, sparameters).AsQueryable();
            }
            else
            {
                return _dataContext.Database.SqlQuery<TEntity>(sprocname).AsQueryable();
            }
        }

        /// <summary>
        /// Calls stored procedure.
        /// </summary>
        /// <param name="sprocname"></param>
        /// <param name="update"></param>
        /// <param name="sparameters"></param>
        public void StoredProc(string sprocname, bool update, params object[] sparameters)
        {
            if (update)
            {
                _dataContext.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sprocname,
                    sparameters);
            }
            else
            {
                _dataContext.Database.SqlQuery<TEntity>(sprocname, sparameters);
            }
        }


        public void Add(TEntity entity)
        {
            _dataContext.Entry(entity).State = EntityState.Added;
        }


        public void Update(TEntity entity)
        {

            _dbSet.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int ID)
        {
            var entity = _dataContext.Set<TEntity>().Find(ID);
            _dataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Dispose()
        {
            if (this._dataContext != null)
            {
                this._dataContext.Dispose();
            }

        }

    }

}


using Castle.Windsor;
using Test_Solution1.DataAccessInterface;
using Test_Solution1.DataAccessInterface.Repository;
using System;
namespace Test_Solution1.DataAccess.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private EFContext _dbContext;
        
        

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>

        private IWindsorContainer _container;
        public UnitOfWork(EFContext context, IWindsorContainer container)
        {
            _dbContext = context;
            _container = container;

        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            // Save changes with the default options
            int returnValue = _dbContext.SaveChanges();
            return returnValue;
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Rollback()
        {
            _dbContext.Dispose();
        }

       


    }
}

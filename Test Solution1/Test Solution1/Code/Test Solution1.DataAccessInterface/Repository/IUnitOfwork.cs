using System;
namespace Test_Solution1.DataAccessInterface.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void Rollback();
        
        

    }
}

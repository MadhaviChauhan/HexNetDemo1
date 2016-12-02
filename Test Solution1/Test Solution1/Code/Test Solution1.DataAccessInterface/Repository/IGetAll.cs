using System.Collections.Generic;

namespace Test_Solution1.DataAccessInterface.Repository
{
    public interface IGetAll<out T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}
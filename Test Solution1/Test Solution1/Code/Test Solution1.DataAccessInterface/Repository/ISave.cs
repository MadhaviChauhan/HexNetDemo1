namespace Test_Solution1.DataAccessInterface.Repository
{
    public interface ISave<in T> where T : class
    {
        void Save(T entity);
    }
}
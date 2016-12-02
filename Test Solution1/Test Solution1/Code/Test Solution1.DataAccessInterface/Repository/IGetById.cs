namespace Test_Solution1.DataAccessInterface.Repository
{
    public interface IGetById<out T> where T : class
    {
        T GetById(int id);
    }
}
namespace Test_Solution1.DataAccess.Context
{
    public class EFContext : EFDBContext
    {
        public EFContext()
            : base()

        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
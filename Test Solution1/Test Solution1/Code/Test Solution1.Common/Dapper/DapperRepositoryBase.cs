namespace Test_Solution1.Common.Dapper
{
    public abstract class DapperRepositoryBase<TContext> where TContext : DbContext, new()
    {


        private TContext _DataContext;

        public virtual TContext DataContext
        {
            get
            {
                if (_DataContext == null)
                {
                    _DataContext = new TContext();
                }
                return _DataContext;
            }
            set
            {
                _DataContext = value;
            }
        }



    }
}
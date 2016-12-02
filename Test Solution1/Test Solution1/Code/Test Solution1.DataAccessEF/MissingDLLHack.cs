using System.Data.Entity.SqlServer;

namespace Test_Solution1.DataAccess
{
    internal static class MissingDllHack
    {
        private static SqlProviderServices instance = SqlProviderServices.Instance;
    }
}

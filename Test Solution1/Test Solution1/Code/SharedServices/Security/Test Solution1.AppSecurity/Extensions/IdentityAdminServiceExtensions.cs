using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using Test_Solution1.AppSecurity.Service;

namespace Test_Solution1.AppSecurity.Extensions
{

    public static class IdentityAdminServiceExtensions
    {
        public static void Configure(this IdentityAdminServiceFactory factory)
        {
            //factory.Register(new Registration<CContext>(resolver => new CContext(connectionString)));
            factory.IdentityAdminService = new Registration<IIdentityAdminService, IdentityAdminManagerService>();
        }
    }

}
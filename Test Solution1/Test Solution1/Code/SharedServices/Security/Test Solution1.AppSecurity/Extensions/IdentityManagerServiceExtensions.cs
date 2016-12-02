using IdentityManager;
using IdentityManager.AspNetIdentity;
using IdentityManager.Configuration;
using Test_Solution1.AppSecurity.Entities;
using Test_Solution1.AppSecurity.Seed_Data;
using System.Collections.Generic;
using System.Linq;

namespace Test_Solution1.AppSecurity.Extensions
{

    public static class IdentityManagerServiceExtensions
    {
        public static void ConfigureIdentityManagerService(this IdentityManagerServiceFactory factory, string connectionString)
        {
            factory.Register(new Registration<CContext>(resolver => new CContext(connectionString)));
            factory.Register(new Registration<CUserStore>());
            factory.Register(new Registration<CRoleStore>());
            factory.Register(new Registration<CUserManager>());
            factory.Register(new Registration<CRoleManager>());
            factory.IdentityManagerService = new Registration<IIdentityManagerService, CIdentityManagerService>();

            ConfigureUser(Users.Get(), connectionString);
        }
        public static void ConfigureUser(IEnumerable<CUser> users, string connectionString)
        {
            using (var db = new CContext(connectionString))
            {
                if (!db.Users.Any())
                {
                    foreach (var u in users)
                    {
                        u.Claims.Add(new CUserClaim { ClaimValue = "UserManagementAdmin", ClaimType = Constants.ClaimTypes.Role });

                        u.Claims.Add(new CUserClaim { ClaimValue = "ClientScopeManagementAdmin", ClaimType = Constants.ClaimTypes.Role });

                        db.Users.Add(u);
                    }

                    db.SaveChanges();
                }
            }
        }
    }

    public class CIdentityManagerService : AspNetIdentityManagerService<CUser, int, CRole, int>
    {
        public CIdentityManagerService(CUserManager userMgr, CRoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }

}
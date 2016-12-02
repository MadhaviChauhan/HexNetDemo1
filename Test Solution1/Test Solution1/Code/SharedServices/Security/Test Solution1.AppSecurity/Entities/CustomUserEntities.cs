using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Test_Solution1.AppSecurity.Entities
{
    public class CUser : IdentityUser<int, CUserLogin, CUserRole, CUserClaim>
    {
        public int? TenantId { get; set; }
    }
    public class CUserLogin : IdentityUserLogin<int> { }
    public class CUserRole : IdentityUserRole<int> { }
    public class CUserClaim : IdentityUserClaim<int> { }

    public class CRole : IdentityRole<int, CUserRole> { }

    public class CContext : IdentityDbContext<CUser, CRole, int, CUserLogin, CUserRole, CUserClaim>
    {
        public CContext(string connString)
            : base(connString)
        {
        }
    }

    public class CUserStore : UserStore<CUser, CRole, int, CUserLogin, CUserRole, CUserClaim>
    {
        public CUserStore(CContext ctx)
            : base(ctx)
        {
        }
    }

    public class CUserManager : UserManager<CUser, int>
    {
        public CUserManager(CUserStore store)
            : base(store)
        {
        }
    }

    public class CRoleStore : RoleStore<CRole, int, CUserRole>
    {
        public CRoleStore(CContext ctx)
            : base(ctx)
        {
        }
    }

    public class CRoleManager : RoleManager<CRole, int>
    {
        public CRoleManager(CRoleStore store)
            : base(store)
        {
        }
    }

}
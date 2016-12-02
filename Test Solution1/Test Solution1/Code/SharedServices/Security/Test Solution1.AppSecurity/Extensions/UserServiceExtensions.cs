
using IdentityServer3.AspNetIdentity;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Test_Solution1.AppSecurity.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Solution1.AppSecurity.Extensions
{
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString)
        {
            factory.UserService = new Registration<IUserService, CUserService>();
            factory.Register(new Registration<CUserManager>());
            factory.Register(new Registration<CUserStore>());
            factory.Register(new Registration<CContext>(resolver => new CContext(connString)));
        }
    }


    public class CUserService : AspNetIdentityUserService<CUser, int>
    {
        public CUserService(CUserManager userMgr)
            : base(userMgr)
        {
        }
        protected override async Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsFromAccount(CUser user)
        {
            var claims = (await base.GetClaimsFromAccount(user)).ToList();
            if (user.TenantId.HasValue)
            {
                claims.Add(new System.Security.Claims.Claim("Tenant_Id", user.TenantId.ToString()));
            }

            return claims;
        }
    }
}
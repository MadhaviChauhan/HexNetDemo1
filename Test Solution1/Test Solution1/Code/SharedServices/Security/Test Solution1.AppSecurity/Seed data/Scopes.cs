using IdentityManager;
using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Test_Solution1.AppSecurity.Seed_Data
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
        {
            new Scope
            {
                Enabled = true,
                Name = "roles",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("role")
                }
            },
            new Scope{
                    Name = Constants.IdMgrScope,
                    DisplayName = "IdentityManager",
                    Description = "Authorization for IdentityManager",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>{
                        new ScopeClaim(Constants.ClaimTypes.Name),
                        new ScopeClaim(Constants.ClaimTypes.Role)
                    }
            },
            new Scope{
                    Name = "idmAdmgr",
                    DisplayName = "IdentityAdmin",
                    Description = "Authorization for IdentityAdmin",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>{
                        new ScopeClaim(Constants.ClaimTypes.Name),
                        new ScopeClaim(Constants.ClaimTypes.Role)
                    }
             }
        };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}
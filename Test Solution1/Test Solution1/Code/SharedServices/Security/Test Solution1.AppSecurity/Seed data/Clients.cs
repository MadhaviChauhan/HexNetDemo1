using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Test_Solution1.AppSecurity.Seed_Data
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
            new Client
            {
                Enabled = true,
                ClientName = "MVC Client",
                ClientId = "mvc",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    @"http://localhost:57012/"
                },
                AllowAccessToAllScopes = true

            },
             new Client
            {
                Enabled = true,
                ClientName = "User Management",
                ClientId = "idmgr",
                AllowedScopes = new List<string> {"idmgr" },
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    @"https://localhost:44300/UserManagement/"
                },
                AllowAccessToAllScopes = true

            },
              new Client{
                    ClientId = "idmAdmgr_client",
                    ClientName = "IdentityAdmin",
                    Enabled = true,
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    AllowedScopes = new List<string> {"idmAdmgr","openid", "profile","roles" },
                    RedirectUris = new List<string>{
                        "https://localhost:44300/Admin/"
                    },
                    IdentityProviderRestrictions = new List<string>(){Constants.PrimaryAuthenticationType}
                }
        };
        }
    }
}
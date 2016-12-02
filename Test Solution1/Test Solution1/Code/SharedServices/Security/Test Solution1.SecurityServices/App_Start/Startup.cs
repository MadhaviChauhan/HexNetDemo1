using IdentityAdmin.Configuration;
using IdentityManager.Configuration;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Test_Solution1.AppSecurity.Extensions;
using Test_Solution1.AppSecurity.Service;
using Serilog;
using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Test_Solution1.SecurityServices.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // change with your desired log level
            .WriteTo.File(@"C:\temp\myPath.txt") // remember to assign proper writing privileges on the file
            .CreateLogger();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"

            });

            app.Map("/identity", idsrvApp =>
            {
                var options = new IdentityServerOptions
                {
                    SiteName = "Security Token Server",
                    SigningCertificate = LoadCertificate(),
                    Factory = IdSrvFactory.Configure("SecurityTokenServiceConfig")
                };

                idsrvApp.UseIdentityServer(options);

            });

            app.Map("/UserManagement", adminApp =>
            {
                adminApp.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    Authority = "https://localhost:44300/identity",
                    ClientId = "idmgr",
                    Scope = "openid profile roles idmgr",
                    RedirectUri = @"https://localhost:44300/UserManagement/",
                    ResponseType = "id_token",
                    SignInAsAuthenticationType = "Cookies",
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = n =>
                        {
                            return AddClaims(n);
                        }
                    }
                });
                var factory = new IdentityManagerServiceFactory();
                factory.ConfigureIdentityManagerService("SecurityTokenServiceConfig");

                adminApp.UseIdentityManager(new IdentityManagerOptions()
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies",
                        AdminRoleName = "UserManagementAdmin"
                    }
                });
            });

            app.Map("/Admin", adminApp =>
            {
                adminApp.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    Authority = "https://localhost:44300/identity",
                    ClientId = "idmAdmgr_client",
                    Scope = "openid profile roles idmAdmgr",
                    RedirectUri = @"https://localhost:44300/Admin/",
                    ResponseType = "id_token",
                    SignInAsAuthenticationType = "Cookies",
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = n =>
                        {
                            return AddClaims(n);
                        }
                    }
                });
                var factory = new IdentityAdminServiceFactory();
                factory.Configure();
                adminApp.UseIdentityAdmin(new IdentityAdminOptions
                {
                    Factory = factory,
                    AdminSecurityConfiguration = new AdminHostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies",
                        AdminRoleName = "ClientScopeManagementAdmin"
                    }
                });
            });
        }

        private static Task AddClaims(Microsoft.Owin.Security.Notifications.SecurityTokenValidatedNotification<Microsoft.IdentityModel.Protocols.OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> n)
        {
            n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
            foreach (var claim in n.AuthenticationTicket.Identity.FindAll(x => x.Type == ClaimTypes.Role))
                n.AuthenticationTicket.Identity.AddClaim(new Claim("role", claim.Value));
            return Task.FromResult(0);
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\certificates\localhost.pfx", AppDomain.CurrentDomain.BaseDirectory), "secret");
        }
    }
}
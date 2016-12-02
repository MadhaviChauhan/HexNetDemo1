using Test_Solution1.ApplicationServices.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Test_Solution1.ServiceAgents.Installers
{
    public class ServiceAgentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(
                Classes.FromThisAssembly()
                    .IncludeNonPublicTypes()
                    .Pick()
                    .WithServiceBase().WithServiceAllInterfaces()
                    .LifestyleTransient()
                    .Configure(c => c.LifestyleTransient()
                    )
                );


            container.Install(FromAssembly.Containing(typeof(ApiInstaller)));

        }
    }
}
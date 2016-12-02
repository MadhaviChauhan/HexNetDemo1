using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Caching.Installers
{
    public class CachingInstaller : IWindsorInstaller
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




        }
    }
}
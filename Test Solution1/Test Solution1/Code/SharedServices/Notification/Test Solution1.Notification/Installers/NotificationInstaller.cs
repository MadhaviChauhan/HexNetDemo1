using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Test_Solution1.Notification.Installers
{
    public class NotificationInstaller : IWindsorInstaller
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
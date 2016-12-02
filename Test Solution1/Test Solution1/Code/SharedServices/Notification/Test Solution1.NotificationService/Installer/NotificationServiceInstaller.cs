using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Test_Solution1.Notification.Installers;
using System.Web.Http.Controllers;

namespace Test_Solution1.NotificationService.Installer
{

    public class NotificationServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());

            container.Install(FromAssembly.Containing(typeof(NotificationInstaller)));
        }
    }
}
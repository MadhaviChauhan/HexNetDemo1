using Caching.Installers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http.Controllers;

namespace CachingService.Installer
{

    public class CachingServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());

            container.Install(FromAssembly.Containing(typeof(CachingInstaller)));
        }
    }
}
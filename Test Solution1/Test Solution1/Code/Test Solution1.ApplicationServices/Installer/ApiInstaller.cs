using Test_Solution1.BusinessServices.Installers;
using Caching;
using Caching.Interface;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http.Controllers;

namespace Test_Solution1.ApplicationServices.Installer
{

    public class ApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<CacheFacility>();
            container.Register(Component.For<ICacheProvider>().ImplementedBy<NetCacheProvider>());

            container.Register(Types.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());
            container.Install(FromAssembly.Containing(typeof(ServiceInstaller)));






        }
    }
}
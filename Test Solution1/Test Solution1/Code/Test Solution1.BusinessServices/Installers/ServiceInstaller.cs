using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Test_Solution1.Common.Interceptor;
using Test_Solution1.DataAccess.Installers;
using ServicesGateway.Installers;

namespace Test_Solution1.BusinessServices.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(
                Classes.FromThisAssembly()
                    .IncludeNonPublicTypes()
                    .Pick()
                    .WithServiceBase().WithServiceAllInterfaces()
                    .LifestyleTransient()
                    .Configure(component => component.Interceptors<LoggingInterceptorCastle>()
                    )
                );


            container.Install(FromAssembly.Containing(typeof(DataInstaller)));
            container.Install(FromAssembly.Containing(typeof(ServiceGatewayInstaller)));

        }
    }
}
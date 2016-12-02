
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Test_Solution1.Common.Interceptor;
using Test_Solution1.DataAccess.Context;
using Test_Solution1.DataAccessInterface.Repository;
namespace Test_Solution1.DataAccess.Installers
{
    public class DataInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            //Register CLasses other than IRepository
            container.Register(Classes.FromAssembly(typeof(DataInstaller).Assembly)
                                .BasedOn(typeof(IRepository<>), typeof(IUnitOfWork), typeof(EFContext))
                                .WithService.Base()
                                .WithServiceAllInterfaces()
                                .LifestyleTransient().Configure(component => component.Interceptors<LoggingInterceptorCastle>())
                                );
            //Register BaseRpository

            //Register  LoggingInterceptorCastle
            container.Register(Component.For<IInterceptor>()
                     .ImplementedBy<LoggingInterceptorCastle>());
            //Register Container to inject into Unit of Work
            container.Register(Component.For<IWindsorContainer>().Instance(container));
        }
    }
}
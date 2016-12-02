using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http;
using WebAPI.Container;

namespace Test_Solution1.CachingService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            ConfigureWindsor(GlobalConfiguration.Configuration);
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }
    }
}

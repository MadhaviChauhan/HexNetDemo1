using Test_Solution1.ApplicationServices.Container;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Configuration;
using System.Web.Http;
using System.Web.Routing;

namespace Test_Solution1.ApplicationServices
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        protected void Application_Start()
        {
            //GlobalConfiguration.Configuration.Filters.Add(new ElmahHandleErrorApiAttribute());
            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            ConfigureWindsor(GlobalConfiguration.Configuration);


        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["APPNAME"]; if (cs.ProviderName == "MySql.Data.MySqlClient"){ System.Data.Entity.DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration()); } }
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

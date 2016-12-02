using Elmah.Contrib.WebApi;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Test_Solution1.ApplicationServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GetOrdersAllOptional",
                routeTemplate: "api/{controller}/{action}/{status}/{orderNo}/{customer}/{orderDateFrom}/{orderDateTo}/{salesPerson}/{carrier}",
                constraints: new
                {
                    orderDateFrom = @"20\d\d-[0-1]?\d-[0-3]?\d",
                    orderDateTo = @"20\d\d-[0-1]?\d-[0-3]?\d"
                },
                defaults: new
                {
                    controller = "Order",
                    action = "GetOrders",
                    orderNo = RouteParameter.Optional,
                    customer = RouteParameter.Optional,
                    orderDateFrom = RouteParameter.Optional,
                    orderDateTo = RouteParameter.Optional,
                    salesPerson = RouteParameter.Optional,
                    carrier = RouteParameter.Optional,
                    status = RouteParameter.Optional
                }

                );
            //config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.Routes.MapHttpRoute("AXD", "{resource}.axd/{*pathInfo}", null, null, new StopRoutingHandler());

            config.Routes.MapHttpRoute(
                name: "GetOrdersBasedonSort",
                routeTemplate: "api/{controller}/{action}/{status}/{orderNo}/{customer}/{orderDateFrom}/{orderDateTo}/{salesPerson}/{carrier}/{sortObject}",
                constraints: new
                {
                    orderDateFrom = @"20\d\d-[0-1]?\d-[0-3]?\d",
                    orderDateTo = @"20\d\d-[0-1]?\d-[0-3]?\d"
                },
                defaults: new
                {
                    controller = "Order",
                    action = "GetOrders",
                    orderNo = RouteParameter.Optional,
                    customer = RouteParameter.Optional,
                    orderDateFrom = RouteParameter.Optional,
                    orderDateTo = RouteParameter.Optional,
                    salesPerson = RouteParameter.Optional,
                    carrier = RouteParameter.Optional,
                    status = RouteParameter.Optional,
                    sortObject = RouteParameter.Optional
                }

                );

            config.Routes.MapHttpRoute(
                name: "GetOrderDetails",
                routeTemplate: "api/{controller}/{action}/{orderNo}"
                );

            config.Routes.MapHttpRoute(
                name: "GetCustomersByFilter",
                routeTemplate: "api/{controller}/{action}/{filterObject}/{sortObject}"
                );

            config.Filters.Add(new ElmahHandleErrorApiAttribute());
        }


    }
}


using System.Web.Mvc;
using System.Web.Routing;

namespace Demo.Web
{
    public class RouteConfig
    {
        /// <summary>
        /// Registers MVC routes. Each AngularJS module has it's own controller and route. When the url for
        /// the particular module is hit, ASPNET will load the appropriate html which will load the Angular module
        /// and then the module will take over routing using ui-router
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CatchAll",
                url: "{*clientRoute}",
               defaults: new { controller = "App", action = "Index" }
            );

        }
    }
}

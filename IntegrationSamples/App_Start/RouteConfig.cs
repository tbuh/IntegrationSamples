using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IntegrationSamples
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Register the default hubs route: ~/signalr/hubs

            routes.MapRoute(
                name: "signin-Purecloud",
                url: "signin-Purecloud",
                defaults: new { controller = "Home", action = "Social" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Social", id = UrlParameter.Optional }
            );
        }
    }
}

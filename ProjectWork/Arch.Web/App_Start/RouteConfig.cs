using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Arch.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "",
                defaults: new { controller = "Dashboard", action = "Index" }
            );
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Login", action = "Index"}
            );
            routes.MapRoute(
                name: "ViewerWithName",
                url: "ViewerWithName/{id}/{fileName}",
                defaults: new { controller = "Media", action = "ViewerWithName", id = UrlParameter.Optional, fileName = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

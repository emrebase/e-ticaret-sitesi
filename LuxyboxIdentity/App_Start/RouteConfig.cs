using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LuxyboxIdentity
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });


            routes.MapRoute(
                name: "Categoriess",
                url: "Home/{Products}/{categorytId}",
                new { controller = "Home", action = "Products", categoryId = UrlParameter.Optional });

            routes.MapRoute(
              name: "Products",
              url: "Home/{Details}/{productId}",
              new { controller = "Home", action = "Details", productId = UrlParameter.Optional });



            //routes.MapRoute(
            //    name: "Products",
            //    url: "{Home}/{action}/{Products}",
            //    new { controller = "Home", action = "Products", id = UrlParameter.Optional }
            //);
            //routes.MapRoute(
            //    name: "Details",
            //    url: "{Home}/{action}/{Details}",
            //    new { controller = "Home", action = "Products", id = UrlParameter.Optional }
            //);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using TicketDesk.Web.Filters;

namespace TicketDesk.Web
{
    public static class RouteConfig
    {
        public static void RegisterWebApiRoutes(HttpConfiguration config)
        {
            //Use web api routing
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "TextApi",
               routeTemplate: "api/text/{lang}/{ns}",
               defaults: new { controller = "Text" }
           );

            config.Routes.MapHttpRoute(
                name: "BreezeDefault",
                routeTemplate: "breeze/{action}",
                defaults: new { Controller = "Metadata" }
            );

            config.Routes.MapHttpRoute(
                name: "BreezeModule",
                routeTemplate: "breeze/{controller}/{action}"
            );

            config.EnableQuerySupport();
        }

        public static void RegisterMVCRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Sitemap", 
                url: "sitemap",
                defaults: new { controller = "Sitemap", action = "Sitemap" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
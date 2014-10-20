using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public static class HtmlHelperExtensions
    {
        public static string IsActiveRoute(this HtmlHelper html, string controllerName, string actionName)
        {
            var action = html.ViewContext.RouteData.Values["action"] as string;
            var controller = html.ViewContext.RouteData.Values["controller"] as string;

            return (controllerName == controller && actionName == action) ? "active" : "";
        }
    }
}
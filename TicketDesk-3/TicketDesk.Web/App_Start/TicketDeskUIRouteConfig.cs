using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(TicketDesk.Web.App_Start.TicketDeskUIRouteConfig), "PreStart", Order = 2)]

namespace TicketDesk.Web.App_Start
{
    ///<summary>
    /// Inserts the HotTowel SPA sample view controller to the front of all MVC routes
    /// so that the HotTowel SPA sample becomes the default page.
    ///</summary>
    ///<remarks>
    /// This class is discovered and run during startup
    /// http://blogs.msdn.com/b/davidebb/archive/2010/10/11/light-up-your-nupacks-with-startup-code-and-webactivator.aspx
    ///</remarks>
    public static class TicketDeskUIRouteConfig
    {

        public static void PreStart()
        {
            //for SignalR
            System.Web.Routing.RouteTable.Routes.MapHubs();
            // Preempt standard default MVC page routing to go to HotTowel Sample
            System.Web.Routing.RouteTable.Routes.MapRoute(
                name: "TicketDeskUI",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "TicketDeskUI",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}
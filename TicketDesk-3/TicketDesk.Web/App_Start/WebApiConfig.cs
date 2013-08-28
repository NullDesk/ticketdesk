using System.Web.Http;
using Newtonsoft.Json;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(TicketDesk.Web.App_Start.WebApiConfig), "RegisterBreezePreStart")]
namespace TicketDesk.Web.App_Start
{
    ///<summary>
    /// Inserts the Breeze Web API controller route at the front of all Web API routes
    ///</summary>
    ///<remarks>
    /// This class is discovered and run during startup; see
    /// http://blogs.msdn.com/b/davidebb/archive/2010/10/11/light-up-your-nupacks-with-startup-code-and-webactivator.aspx
    ///</remarks>
    public static class WebApiConfig
    {

        public static void RegisterBreezePreStart()
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "BreezeApi",
                routeTemplate: "breeze/{controller}/{action}"
            );

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "TextApi",
                routeTemplate: "api/text/{lang}/{ns}",
                defaults: new {controller = "Text"}
            );

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
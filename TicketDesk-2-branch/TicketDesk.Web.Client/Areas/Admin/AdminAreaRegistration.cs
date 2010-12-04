using System.Web.Mvc;

namespace TicketDesk.Web.Client.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.IgnoreRoute("Admin/elmah.axd/{*pathInfo}");

            context.MapRoute(
                "ApplicationSettingsEdit",
                "ApplicationSettings/{action}/{settingName}",
                new { controller = "ApplicationSettings", action = "Edit" });

            context.MapRoute(
                "ApplicationSettingsList",
                "ApplicationSettings/{action}",
                new { controller = "ApplicationSettings", action = "List" });


            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

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
                "SecurityUserList",
                "Admin/SecurityManagement/UsersList/{page}",
                new { controller = "SecurityManagement", action = "UsersList", page = 1 });

            context.MapRoute(
                "SecurityEditUser",
                "Admin/SecurityManagement/EditUser/{userName}",
                new { controller = "SecurityManagement", action = "EditUser" });


            context.MapRoute(
              "SecurityCreateUser",
              "Admin/SecurityManagement/CreateUser/{userName}",
              new { controller = "SecurityManagement", action = "CreateUser" });

            context.MapRoute(
             "SecurityDeleteUser",
             "Admin/SecurityManagement/DeleteUser/{userName}",
             new { controller = "SecurityManagement", action = "DeleteUser" });



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
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

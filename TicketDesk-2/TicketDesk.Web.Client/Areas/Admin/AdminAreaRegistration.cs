// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

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

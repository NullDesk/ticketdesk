// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Configuration;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;
using System.Data.Entity.Migrations;

namespace TicketDesk.Web.Identity.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<TdIdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskIdentity";
        }

        /// <summary>
        /// Seeds the specified context.
        /// </summary>
        /// <remarks>
        /// for whatever reason, the internals for identity call this each time the 
        /// application starts (initialize with force = true). It is imporatnt that
        /// this not do anything that might break existing identity data
        /// </remarks>
        /// <param name="context">The context.</param>
        protected override void Seed(TdIdentityContext context)
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                DemoIdentityDataManager.SetupDemoIdentityData(context);
            }
            else
            {
                InitializeStockUsers(context);
            }
        }


        public static void InitializeStockUsers(TdIdentityContext context)
        {

            //create the standard roles and default admin account
            var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<TicketDeskRole>(context);

            var userManager = new TicketDeskUserManager(userStore);
            var roleManager = new TicketDeskRoleManager(roleStore);
            roleManager.EnsureDefaultRolesExist();
            

            var existingAdminRole = roleManager.FindByName("TdAdministrators");
            //only create default admin user if no other user exists with the admin role
            if (existingAdminRole != null &&
                !userManager.Users.Any(u => u.Roles.Any(r => r.RoleId == existingAdminRole.Id)))
            {
                var admin = new TicketDeskUser
                {
                    Id = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    DisplayName = "Admin User",
                };
                if (userManager.FindById("64165817-9cb5-472f-8bfb-6a35ca54be6a") == null)
                {
                    var adminRoles = new[] { "TdAdministrators", "TdHelpDeskUsers", "TdInternalUsers" };
                    userManager.Create(admin, "123456");

                    foreach (var rname in adminRoles)
                    {
                        userManager.AddToRole(admin.Id, rname);
                    }
                }
            }
        }
    }
}


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
            InitializeUsers(context);
        }

        
        public void InitializeUsers(TdIdentityContext context)
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                DemoIdentityDataManager.SetupDemoIdentityData(context);
            }
            else//TODO: move this block to the context itself, and ensure that at least one admin account exists no matter what
            {
                //create the standard roles and default admin account
                var userStore = new UserStore<TicketDeskUser>(context);
                var roleStore = new RoleStore<IdentityRole>(context);

                //TODO: this user manager has a default config, need to leverage the same user manager as the rest of the application
                var userManager = new UserManager<TicketDeskUser>(userStore);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var roleNames = context.DefaultRoleNames;
                foreach (var roleName in roleNames)
                {
                    //Create Role if it does not exist
                    var role = roleManager.FindByName(roleName);
                    if (role == null)
                    {
                        role = new IdentityRole(roleName);
                        roleManager.Create(role);
                    }
                }

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
                        var adminRoles = new[] {"TdAdministrators", "TdHelpDeskUsers", "TdInternalUsers"};
                        userManager.Create(admin, "123456");

                        userManager.SetLockoutEnabled(admin.Id, false);

                        foreach (var rname in adminRoles)
                        {
                            userManager.AddToRole(admin.Id, rname);
                        }
                    }
                }
            }
        }
    }
}

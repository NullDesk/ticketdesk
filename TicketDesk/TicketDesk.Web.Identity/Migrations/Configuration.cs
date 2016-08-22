// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
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
            AutomaticMigrationDataLossAllowed = true;
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
                DemoIdentityDataManager.SetupDemoIdentityData(context, null);
            }
            else
            {
                InitializeStockRoles(context);
            }
        }


        public static void InitializeStockRoles(TdIdentityContext context)
        {

            //create the standard roles and default admin account
            //var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<TicketDeskRole>(context);

            //var userManager = new TicketDeskUserManager(userStore);
            var roleManager = new TicketDeskRoleManager(roleStore);
            roleManager.EnsureDefaultRolesExist();
           
        }
    }
}


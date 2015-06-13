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

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity.Migrations
{
    public static class DemoIdentityDataManager
    {
        public static void RemoveAllIdentity(TdIdentityContext context)
        {
            foreach (var user in context.Users)
            {
                context.Users.Remove(user);
            }
            foreach (var role in context.Roles)
            {
                context.Roles.Remove(role);
            }
            context.SaveChanges();

            Configuration.InitializeStockUsers(context);
            context.SaveChanges();
        }

        public static void SetupDemoIdentityData(TdIdentityContext context)
        {
            var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<TicketDeskRole>(context);


            //TODO: this user manager has a default config, need to leverage the same user manager as the rest of the application
            var userManager = new UserManager<TicketDeskUser>(userStore);


            var roleManager = new TicketDeskRoleManager(roleStore);
            roleManager.EnsureDefaultRolesExist();

            var admin = new TicketDeskUser { Id = "64165817-9cb5-472f-8bfb-6a35ca54be6a", UserName = "admin@example.com", Email = "admin@example.com", DisplayName = "Admin User" };
            var staff = new TicketDeskUser { Id = "72bdddfb-805a-4883-94b9-aa494f5f52dc", UserName = "staff@example.com", Email = "staff@example.com", DisplayName = "HelpDesk User" };
            var reguser = new TicketDeskUser { Id = "17f78f38-fa68-445f-90de-38896140db28", UserName = "user@example.com", Email = "user@example.com", DisplayName = "Regular User" };
            var users = new[] { admin, staff, reguser };
            var rolesNames = new Dictionary<string, string[]>
            {
                {"admin@example.com", new[] {"TdAdministrators", "TdHelpDeskUsers", "TdInternalUsers"}},
                {"staff@example.com", new[] {"TdHelpDeskUsers", "TdInternalUsers"}},
                {"user@example.com", new[] {"TdInternalUsers"}}
            };
            foreach (var tdUser in users)
            {

                var user = userManager.FindByName(tdUser.UserName);
                if (user == null)
                {
                    user = tdUser;
                    userManager.Create(user, "123456");
                    userManager.SetLockoutEnabled(user.Id, false);
                }
                var rnames = rolesNames[user.UserName];
                var rolesForUser = userManager.GetRoles(user.Id);
                foreach (var rname in rnames.Where(rname => !rolesForUser.Contains(rname)))
                {
                    userManager.AddToRole(user.Id, rname);
                }
            }
        }
    }
}

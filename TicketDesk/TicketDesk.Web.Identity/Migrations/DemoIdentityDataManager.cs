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
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity.Migrations
{
    public static class DemoIdentityDataManager
    {
        public static void RemoveIdentity(TdIdentityContext context, string currentUserId)
        {
            //kill all users and roles except current user
            foreach (var user in context.Users.Where(u => !u.Id.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Users.Remove(user);
            }
            foreach (var role in context.Roles)
            {
                context.Roles.Remove(role);
            }
            context.SaveChanges();

            //re-make the default roles
            Configuration.InitializeStockRoles(context);

            //put the current user back in the admin role
            var userStore = new UserStore<TicketDeskUser>(context);
            var userManager = new TicketDeskUserManager(userStore);
            var currentUser = userManager.FindById(currentUserId);
            if (!userManager.IsInRole(currentUser.Id, "TdAdministrators"))
            {
                userManager.AddToRole(currentUser.Id, "TdAdministrators");
            }
            context.SaveChanges();
        }

        public static void SetupDemoIdentityData(TdIdentityContext context, string currentUserId)
        {
            var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<TicketDeskRole>(context);
            var userManager = new TicketDeskUserManager(userStore);
            var roleManager = new TicketDeskRoleManager(roleStore);

            roleManager.EnsureDefaultRolesExist();
            
            
            var staff = new TicketDeskUser { Id = "72bdddfb-805a-4883-94b9-aa494f5f52dc", UserName = "staff@example.com", Email = "staff@example.com", DisplayName = "HelpDesk User" };
            var reguser = new TicketDeskUser { Id = "17f78f38-fa68-445f-90de-38896140db28", UserName = "user@example.com", Email = "user@example.com", DisplayName = "Regular User" };
            var users = new List<TicketDeskUser> { staff, reguser };
            var rolesNames = new Dictionary<string, string[]>
            {
                {"staff@example.com", new[] {"TdHelpDeskUsers"}},
                {"user@example.com", new[] {"TdInternalUsers"}}
            };

            if (currentUserId == null && userManager.FindByName("admin@example.com") == null)
            {
                users.Add( new TicketDeskUser
                {
                    Id = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    DisplayName = "Admin User"
                });
                rolesNames.Add("admin@example.com", new[] { "TdAdministrators" });
            }


            foreach (var tdUser in users)
            {
                var user = userManager.FindById(tdUser.Id);
                if (user != null)
                {
                    userManager.Delete(user);
                }
                user = tdUser;
                userManager.Create(user, "123456");
           
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

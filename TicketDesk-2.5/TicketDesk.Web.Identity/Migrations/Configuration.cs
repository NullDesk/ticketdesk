using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<TicketDesk.Web.Identity.TicketDeskIdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskIdentity";
        }

        protected override void Seed(TicketDeskIdentityContext context)
        {
            InitializeUsers(context);
        }
        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public void InitializeUsers(TicketDeskIdentityContext context)
        {
            
            var userStore = new UserStore<TicketDeskUser>(context);
            var roleStore = new RoleStore<IdentityRole>(context);
            //TODO: this user manager has a default config, need to leverage the same user manager as the rest of the application
            var userManager = new UserManager<TicketDeskUser>(userStore);

            var roleManager = new RoleManager<IdentityRole>(roleStore);

            const string name = "admin@example.com";
            const string password = "Admin@123456";
            var roleNames = context.DefaultRoleNames;
            const string displayName = "Admin User";
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
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new TicketDeskUser { UserName = name, Email = name, DisplayName = displayName };
                var result = userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("TdAdministrators"))
            {
                userManager.AddToRole(user.Id, "TdAdministrators");
            }
        }
    }
}

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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Client.Annotations;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{

    /// <summary>
    /// Handles security migration functions for upgrades from TD 2.x databases.
    /// </summary>
    public static class LegacySecurityMigrator
    {
        [UsedImplicitly]
        private class LegacyUser
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            internal Guid UserId { get; set; }
            internal string Email { get; set; }
            internal string Password { get; set; }
            internal string Comment { get; set; }
            internal int PasswordFormat { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local

        }

        /// <summary>
        /// Migrates the users and roles from a legacy database to the new TD 2.5 schema.
        /// </summary>
        /// <param name="context">The identity database context</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <returns><c>true</c> if users migrated, <c>false</c> otherwise.</returns>
        public static bool MigrateSecurity(TicketDeskIdentityContext context, TicketDeskUserManager userManager, TicketDeskRoleManager roleManager)
        {
            EnsureRolesExist(roleManager);
            var appId =
                context.Database.SqlQuery<Guid>(
                    "select ApplicationId from aspnet_Applications where ApplicationName = 'TicketDesk'").First().ToString();
            var users = context.Database.SqlQuery<LegacyUser>(
                "select UserId, Email, Password, PasswordFormat, Comment from aspnet_Membership where ApplicationId = '" + appId + "' and IsApproved = 1 and IsLockedOut = 0").ToList();
            const string roleQuery = "SELECT r.RoleName FROM aspnet_UsersInRoles u inner join aspnet_Roles r on u.RoleId = r.RoleId WHERE u.UserId = @userId and r.ApplicationId = @appId";
            
            foreach (var user in users)
            {
                var newUser = new TicketDeskUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    DisplayName = user.Comment,
                };

                var result = user.PasswordFormat == 0 ?
                    userManager.Create(newUser, user.Password) :
                    userManager.Create(newUser);

                if (result.Succeeded)
                {
                    var rolesForUser =
                        context.Database.SqlQuery<string>(roleQuery, 
                        new SqlParameter("userId", user.UserId),
                        new SqlParameter("appId", appId));
                    var newRoles = new List<string>();
                    foreach (var role in rolesForUser)
                    {
                        switch (role.ToLowerInvariant())
                        {
                            case "administrators":
                                newRoles.Add("TdAdministrators");
                                break;
                            case "helpdesk":
                                newRoles.Add("TdHelpDeskUsers");
                                break;
                            case "ticketsubmitters":
                                newRoles.Add("TdInternalUsers");
                                break;
                            default:
                                newRoles.Add("TdPendingUsers");
                                break;
                        }
                    }
                    userManager.AddToRoles(newUser.Id, newRoles.ToArray());
                }
            }
            return true;
        }

        /// <summary>
        /// Ensures the correct set of TD standard roles exist.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        private static void EnsureRolesExist(TicketDeskRoleManager roleManager)
        {
            //TODO: Move this method to td role manager after extending it to use enum role names
            var roleNames = TicketDeskIdentityContext.DefaultRoles;
            foreach (var roleName in roleNames)
            {
                var role = roleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    roleManager.Create(role);
                }
            }
        }
    }
}
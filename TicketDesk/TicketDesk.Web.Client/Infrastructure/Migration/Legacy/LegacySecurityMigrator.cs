// TicketDesk - Attribution notice
//
// Membership object delete scripts from: 
//      dmorrison : https://gist.github.com/dmorrison/942148
//
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
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Domain.Legacy
{

    /// <summary>
    /// Handles security migration functions for upgrades from TD 2.x databases.
    /// </summary>
    public static class LegacySecurityMigrator
    {
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
        public static bool MigrateSecurity(TdIdentityContext context, TicketDeskUserManager userManager, TicketDeskRoleManager roleManager)
        {
            roleManager.EnsureDefaultRolesExist();
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

       

        public static void RemoveLegacyMembershipObjects(TdIdentityContext context)
        {
            const string script = @"
                drop table aspnet_PersonalizationAllUsers
                drop table aspnet_PersonalizationPerUser
                drop table aspnet_Profile
                drop table aspnet_SchemaVersions
                drop table aspnet_UsersInRoles
                drop table aspnet_WebEvent_Events
                drop table aspnet_Paths
                drop table aspnet_Membership
                drop table aspnet_Roles
                drop table aspnet_Users
                drop table aspnet_Applications

                drop view vw_aspnet_Applications
                drop view vw_aspnet_MembershipUsers
                drop view vw_aspnet_Profiles
                drop view vw_aspnet_Roles
                drop view vw_aspnet_Users
                drop view vw_aspnet_UsersInRoles
                drop view vw_aspnet_WebPartState_Paths
                drop view vw_aspnet_WebPartState_Shared
                drop view vw_aspnet_WebPartState_User

                drop procedure aspnet_AnyDataInTables
                drop procedure aspnet_Applications_CreateApplication
                drop procedure aspnet_CheckSchemaVersion
                drop procedure aspnet_Membership_ChangePasswordQuestionAndAnswer
                drop procedure aspnet_Membership_CreateUser
                drop procedure aspnet_Membership_FindUsersByEmail
                drop procedure aspnet_Membership_FindUsersByName
                drop procedure aspnet_Membership_GetAllUsers
                drop procedure aspnet_Membership_GetNumberOfUsersOnline
                drop procedure aspnet_Membership_GetPassword
                drop procedure aspnet_Membership_GetPasswordWithFormat
                drop procedure aspnet_Membership_GetUserByEmail
                drop procedure aspnet_Membership_GetUserByName
                drop procedure aspnet_Membership_GetUserByUserId
                drop procedure aspnet_Membership_ResetPassword
                drop procedure aspnet_Membership_SetPassword
                drop procedure aspnet_Membership_UnlockUser
                drop procedure aspnet_Membership_UpdateUser
                drop procedure aspnet_Membership_UpdateUserInfo
                drop procedure aspnet_Paths_CreatePath
                drop procedure aspnet_Personalization_GetApplicationId
                drop procedure aspnet_PersonalizationAdministration_DeleteAllState
                drop procedure aspnet_PersonalizationAdministration_FindState
                drop procedure aspnet_PersonalizationAdministration_GetCountOfState
                drop procedure aspnet_PersonalizationAdministration_ResetSharedState
                drop procedure aspnet_PersonalizationAdministration_ResetUserState
                drop procedure aspnet_PersonalizationAllUsers_GetPageSettings
                drop procedure aspnet_PersonalizationAllUsers_ResetPageSettings
                drop procedure aspnet_PersonalizationAllUsers_SetPageSettings
                drop procedure aspnet_PersonalizationPerUser_GetPageSettings
                drop procedure aspnet_PersonalizationPerUser_ResetPageSettings
                drop procedure aspnet_PersonalizationPerUser_SetPageSettings
                drop procedure aspnet_Profile_DeleteInactiveProfiles
                drop procedure aspnet_Profile_DeleteProfiles
                drop procedure aspnet_Profile_GetNumberOfInactiveProfiles
                drop procedure aspnet_Profile_GetProfiles
                drop procedure aspnet_Profile_GetProperties
                drop procedure aspnet_Profile_SetProperties
                drop procedure aspnet_RegisterSchemaVersion
                drop procedure aspnet_Roles_CreateRole
                drop procedure aspnet_Roles_DeleteRole
                drop procedure aspnet_Roles_GetAllRoles
                drop procedure aspnet_Roles_RoleExists
                drop procedure aspnet_Setup_RemoveAllRoleMembers
                drop procedure aspnet_Setup_RestorePermissions
                drop procedure aspnet_UnRegisterSchemaVersion
                drop procedure aspnet_Users_CreateUser
                drop procedure aspnet_Users_DeleteUser
                drop procedure aspnet_UsersInRoles_AddUsersToRoles
                drop procedure aspnet_UsersInRoles_FindUsersInRole
                drop procedure aspnet_UsersInRoles_GetRolesForUser
                drop procedure aspnet_UsersInRoles_GetUsersInRoles
                drop procedure aspnet_UsersInRoles_IsUserInRole
                drop procedure aspnet_UsersInRoles_RemoveUsersFromRoles
                drop procedure aspnet_WebEvent_LogEvent

                drop schema aspnet_Membership_FullAccess
                drop schema aspnet_Membership_BasicAccess
                drop schema aspnet_Membership_ReportingAccess
                drop schema aspnet_Personalization_BasicAccess
                drop schema aspnet_Personalization_FullAccess
                drop schema aspnet_Personalization_ReportingAccess
                drop schema aspnet_Profile_BasicAccess
                drop schema aspnet_Profile_FullAccess
                drop schema aspnet_Profile_ReportingAccess
                drop schema aspnet_Roles_BasicAccess
                drop schema aspnet_Roles_FullAccess
                drop schema aspnet_Roles_ReportingAccess
                drop schema aspnet_WebEvent_FullAccess

                drop role aspnet_Membership_FullAccess
                drop role aspnet_Membership_BasicAccess
                drop role aspnet_Membership_ReportingAccess
                drop role aspnet_Personalization_FullAccess
                drop role aspnet_Personalization_BasicAccess
                drop role aspnet_Personalization_ReportingAccess
                drop role aspnet_Profile_FullAccess
                drop role aspnet_Profile_BasicAccess
                drop role aspnet_Profile_ReportingAccess
                drop role aspnet_Roles_FullAccess
                drop role aspnet_Roles_BasicAccess
                drop role aspnet_Roles_ReportingAccess
                drop role aspnet_WebEvent_FullAccess
            ";

            context.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction, script);
        }
    }
}
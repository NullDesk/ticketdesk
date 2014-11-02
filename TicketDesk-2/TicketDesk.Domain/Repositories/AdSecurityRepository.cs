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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Security;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Services;

namespace TicketDesk.Domain.Repositories
{
    [Export(typeof(ISecurityRepository))]
    [ExportMetadata("SecurityMode", "AD")]
    public class AdSecurityRepository : SecurityRepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdSecurityRepository"/> class.
        /// </summary>
        /// <param name="roleProvider">The role provider.</param>
        [ImportingConstructor]
        public AdSecurityRepository(RoleProvider roleProvider) : base(roleProvider){}


        [Import]
        internal AdSecurityCacheService AdService { get; set; }

        
        //public Action RefreshSecurityCache { get { return AdService.RefreshAdSqlCaches; } }

       
        /// <summary>
        /// Formats the name of the user for use in TicketDesk.
        /// </summary>
        /// <param name="unformattedName">The unformatted username (usually from an authentication provider or IPrincipal).</param>
        /// <returns></returns>
        public override string FormatUserName(string unformattedName)
        {
            if (!string.IsNullOrEmpty(unformattedName) && unformattedName.Contains('\\'))
            {
                unformattedName = unformattedName.Split('\\')[1];
            }
            return unformattedName.ToLower();
        }

        /// <summary>
        /// Gets the display name for the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override string GetUserDisplayName(string userName)
        {
            var adDisplayName = AdService.GetAdUserProperty(userName, "displayName");
            if (string.IsNullOrEmpty(adDisplayName))
            {
                adDisplayName = userName;
            }
            return adDisplayName;
        }

        public override bool IsUserInRoleName(string userName, string roleName)
        {
            var members = GetUsersInRole(roleName);
            return members.Any(m => m.Name == userName);
        }

        public override UserInfo[] GetUsersInRole(string roleName)
        {
            string groupName = roleName;//just get the group name without any domain portion
            return AdService.GetRoleMembers(groupName);
        }

        public override string GetUserEmailAddress(string userName)
        {
            return AdService.GetAdUserProperty(userName, "mail");
        }

        public override void AddUserToRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUserFromRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
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
using System.Linq;
using System.Text;
using TicketDesk.Domain.Models;
using System.Runtime.Caching;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel.Composition;
using System.DirectoryServices;

namespace TicketDesk.Domain.Repositories
{
    /// <summary>
    /// Handles communications with AD for user properties and group membership lists. 
    /// </summary>
    [Export]
	internal class AdDataRepository
    {
        [ImportingConstructor]
        internal AdDataRepository
        (
                [Import("ActiveDirectoryDomain")] Func<string> getActiveDirectoryDomainMethod,
                [Import("ActiveDirectoryUser")] Func<string> getActiveDirectoryUserMethod,
                [Import("ActiveDirectoryUserPassword")] Func<string> getActiveDirectoryUserPasswordMethod
        )
        {
            GetActiveDirectoryDomain = getActiveDirectoryDomainMethod;
            GetActiveDirectoryUser = getActiveDirectoryUserMethod;
            GetActiveDirectoryUserPassword = getActiveDirectoryUserPasswordMethod;
        }

        private Func<string> GetActiveDirectoryDomain { get; set; }
        private Func<string> GetActiveDirectoryUser { get; set; }
        private Func<string> GetActiveDirectoryUserPassword { get; set; }





        #region AD Access Members

        internal string GetUserPropertyFromAd(string userName, string propertyName, out bool userFound)
        {
            userFound = false;
            string propertyValue = null;
            using
            (
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain,
                                        GetActiveDirectoryDomain(),
                                        GetActiveDirectoryUser(),
                                        GetActiveDirectoryUserPassword())
            )
            {
                using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userName.ToLowerInvariant()))
                {
                    if (userPrincipal != null)
                    {

                        DirectoryEntry user = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
                        if (user != null)
                        {
                            userFound = true;
                            PropertyValueCollection col = user.Properties[propertyName];
                            if (col != null && col.Count > 0)
                            {
                                propertyValue = col[0].ToString();

                            }
                        }

                    }

                }
            }
            return propertyValue;
        }


        internal UserInfo[] GetGroupMembersFromAd(string groupName)
        {
            UserInfo[] usersInGroup = null;
            using
            (
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain,
                                        GetActiveDirectoryDomain(),
                                        GetActiveDirectoryUser(),
                                        GetActiveDirectoryUserPassword())
            )
            {

                using (GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, groupName.ToLowerInvariant()))
                {
                    if (grp != null)
                    {
                        var x = from p in grp.GetMembers(true)
                                where p.StructuralObjectClass == "user" &&
                                      !string.IsNullOrEmpty(p.UserPrincipalName) &&
                                      !string.IsNullOrEmpty(p.DisplayName) &&
                                      p.UserPrincipalName.Trim() != string.Empty &&
                                      p.DisplayName.Trim() != string.Empty
                                orderby p.DisplayName
                                select new UserInfo() { Name = p.SamAccountName.ToLowerInvariant(), DisplayName = p.DisplayName };

                        usersInGroup = x.ToArray();
                    }
                    else
                    {
                        usersInGroup = new UserInfo[0];//zero length array for memory cache, so it doesn't attempt re-fetches
                    }
                }
            }
            return usersInGroup;
        }

        #endregion






    }
}

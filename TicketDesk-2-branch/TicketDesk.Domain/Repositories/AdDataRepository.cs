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
                using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userName))
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

                using (GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, groupName))
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
                                select new UserInfo(p.SamAccountName.ToLower(), p.DisplayName);

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

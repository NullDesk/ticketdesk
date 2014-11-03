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
using System.ComponentModel.Composition;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Repositories
{
    /// <summary>
    /// Handles SQL communications for the cached values from AD
    /// </summary>
    [Export]
    internal class AdSqlCacheRepository
    {
        [ImportingConstructor]
        internal AdSqlCacheRepository
        (
            AdDataRepository adRepository,
            [Import("StaffRoleName")]string staffRoleName,
            [Import("SubmitterRoleName")]string submitterRoleName,
            [Import("AdminRoleName")]string adminRoleName,
            [Import("AdUserPropertiesSqlCacheRefreshMinutes")] int adUserPropertiesSqlCacheRefreshMinutes
        )
        {
            AdRepository = adRepository;
            TdStaffRoleName = staffRoleName;
            TdSubmittersRoleName = submitterRoleName;
            TdAdminRoleName = adminRoleName;
            AdUserPropertiesSqlCacheRefreshMinutes = adUserPropertiesSqlCacheRefreshMinutes;
        }

        private string TdStaffRoleName { get; set; }
        private string TdSubmittersRoleName { get; set; }
        private string TdAdminRoleName { get; set; }
        private int AdUserPropertiesSqlCacheRefreshMinutes { get; set; }

        internal AdDataRepository AdRepository { get; private set; }

        #region Cache Management

        private static bool isBuildUserPropertiesCacheFirstRun = true;

      
        internal void RefreshSqlCacheFromAd()
        {
            
            BuildRoleMembersCache();
            EnsureStandardPropertiesForAllKnownUsers();
           
            if (isBuildUserPropertiesCacheFirstRun)
            {
                //we only do this once per app instantiation, just in case an AD user is re-created 
                //  this will reactivate those users in SQL cache and update their properties.
                isBuildUserPropertiesCacheFirstRun = false;

                BuildUserPropertiesCache(true);
            }
            else
            {
                BuildUserPropertiesCache(false);
            }
        }

        private void BuildRoleMembersCache()
        {
            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                var configuredTdRoles = new string[] { TdStaffRoleName, TdSubmittersRoleName, TdAdminRoleName };
                var sqlCacheMembers = ctx.AdCachedRoleMembers.ToList();//go ahead and fetch entire table
                foreach (var tdRole in configuredTdRoles)
                {
                    List<UserInfo> adUsersForTdRole = new List<UserInfo>();

                    adUsersForTdRole.AddRange(AdRepository.GetGroupMembersFromAd(tdRole));

                    foreach (var sMember in sqlCacheMembers.Where(rm => rm.GroupName == tdRole))//find roles in SQL that aren't in AD anymore
                    {
                        if (adUsersForTdRole.Count(au => au.Name == sMember.MemberName) < 1)
                        {
                            ctx.AdCachedRoleMembers.DeleteObject(sMember);
                        }
                    }
                    foreach (var aMember in adUsersForTdRole)// file roles in AD that need to be updated/added to SQL
                    {
                        var sMember = sqlCacheMembers.SingleOrDefault(sm => sm.GroupName == tdRole && sm.MemberName == aMember.Name);
                        if (sMember == null)//doesn't exist in SQL, insert
                        {
                            sMember = new AdCachedRoleMember();
                            sMember.GroupName = tdRole;
                            ctx.AdCachedRoleMembers.AddObject(sMember);
                        }
                        sMember.MemberName = aMember.Name;
                        sMember.MemberDisplayName = aMember.DisplayName;
                    }
                    ctx.SaveChanges();
                }
            }
        }

       

        private void BuildUserPropertiesCache(bool includeInactiveAd)
        {
            var refreshTime = DateTime.Now.AddMinutes((AdUserPropertiesSqlCacheRefreshMinutes * -1));
            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                var propertiesForRefresh = ctx.AdCachedUserProperties.Where(up => !up.LastRefreshed.HasValue || up.LastRefreshed.Value <= refreshTime);
                if(!includeInactiveAd)
                { 
                    propertiesForRefresh = propertiesForRefresh.Where(up => up.IsActiveInAd);
                }
                foreach(var property in propertiesForRefresh.ToList())
                {
                    bool userFound;
                    var value = AdRepository.GetUserPropertyFromAd(property.UserName, property.PropertyName, out userFound);
                    property.IsActiveInAd = userFound;
                    if (userFound)//if the user wasn't found, we don't alter the last value fetched (last fetched value lives forever)
                    {
                        property.PropertyValue = value;
                    }
                    property.LastRefreshed = DateTime.Now;
                    ctx.SaveChanges();
                }
                
            }
        }

        private void EnsureStandardPropertiesForAllKnownUsers()
        {
            var allKnownUsers = GetAllKnownDistinctUserNamesFrom();
            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                //go ahead and fetch entire table. The table is kinda big, but realistically 
                //  there should only be two rows per valid user... even with a thousand users, 
                //  that's not an obsurde amount of data. 
                var existingUserProperties = ctx.AdCachedUserProperties.ToList();


                //loop 1 to add all display name properties for any users not already in the SQL cache

                var existingProps = existingUserProperties.Where(ep => ep.PropertyName == "displayName");

                foreach (var knownUser in allKnownUsers)
                {
                    if (existingProps.Count(ep => string.Equals(ep.UserName, knownUser, StringComparison.InvariantCultureIgnoreCase)) < 1)
                    {
                        var newUserProp = new AdCachedUserProperty()
                                            {
                                                UserName = knownUser,
                                                PropertyName = "displayName",
                                                IsActiveInAd = true
                                            };
                        ctx.AdCachedUserProperties.AddObject(newUserProp);
                    }
                }

                //loop 2 to add all email properties for any users not already in the SQL cache
                existingProps = existingUserProperties.Where(ep => ep.PropertyName == "mail");

                foreach (var knownUser in allKnownUsers)
                {
                    if (existingProps.Count(ep => string.Equals(ep.UserName, knownUser, StringComparison.InvariantCultureIgnoreCase)) < 1)
                    {
                        var newUserProp = new AdCachedUserProperty()
                                            {
                                                UserName = knownUser,
                                                PropertyName = "mail",
                                                IsActiveInAd = true
                                            };
                        ctx.AdCachedUserProperties.AddObject(newUserProp);
                    }


                   
                }
                ctx.SaveChanges();
            }
        }

        private IEnumerable<string> GetAllKnownDistinctUserNamesFrom()
        {
            List<string> allUsers = new List<string>();
    
            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                //distincting each one of these just reduces the amount of data coming back from SQL
                //  it doesn't actually give us a truly distinct master list though.
                allUsers.AddRange(ctx.AdCachedRoleMembers.Select(r => r.MemberName.ToLower()).Distinct());
                allUsers.AddRange(ctx.Tickets.Select(t => t.CreatedBy.ToLower()).Distinct());
                allUsers.AddRange(ctx.Tickets.Select(t => t.Owner.ToLower()).Distinct());
                allUsers.AddRange(ctx.Tickets.Select(t => t.AssignedTo.ToLower()).Distinct());
                allUsers.AddRange(ctx.Tickets.Select(t => t.CurrentStatusSetBy.ToLower()).Distinct());
                allUsers.AddRange(ctx.Tickets.Select(t => t.LastUpdateBy.ToLower()).Distinct());
                allUsers.AddRange(ctx.TicketComments.Select(tc => tc.CommentedBy.ToLower()).Distinct());
                allUsers.AddRange(ctx.TicketAttachments.Select(ta => ta.UploadedBy.ToLower()).Distinct());


            }
            var badUsers = allUsers.Where(u => string.IsNullOrEmpty(u)).ToArray();
            foreach (var badUser in badUsers)
            {
                allUsers.Remove(badUser);
            }
            return allUsers.Distinct();//this gets the truly distinct list
          


        }

        #endregion

        #region Demand Cache Access
        /// <summary>
        /// Gets the user property from the SQL store.
        /// </summary>
        /// <remarks>
        /// If the SQL store doesn't contain data for the user/property requested it will
        /// add the user/property to the table so it gets fetched next time the system 
        /// refreshes from AD. In the meantime though, the method will return null.
        /// </remarks>
        /// <param name="userName">Name of the user.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property stored in the SQL cache; null if no value cached</returns>
        internal string GetUserProperty(string userName, string propertyName)
        {
            userName = userName.ToLowerInvariant();
            string propertyValue = null;

            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                var val = ctx.AdCachedUserProperties.FirstOrDefault(p => p.UserName == userName && p.PropertyName == propertyName);

                if (val == null)
                {
                    //add this property/user to the table for the next automated refresh
                    var newUserProp = new AdCachedUserProperty()
                                        {
                                            UserName = userName,
                                            PropertyName = propertyName,
                                            PropertyValue = null,
                                            LastRefreshed = null,
                                            IsActiveInAd = true
                                        };
                    ctx.AdCachedUserProperties.AddObject(newUserProp);
                }
                else
                {
                    propertyValue = val.PropertyValue;
                }
                ctx.SaveChanges();
            }

            return propertyValue;
        }

        /// <summary>
        /// Gets the group members.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        internal UserInfo[] GetGroupMembers(string groupName)
        {
            groupName = groupName.ToLowerInvariant();
            UserInfo[] usersInGroup = null;
            using (TicketDeskEntities ctx = new TicketDeskEntities())
            {
                var users = from m in ctx.AdCachedRoleMembers
                            where m.GroupName == groupName
                            select new UserInfo() { Name = m.MemberName, DisplayName = m.MemberDisplayName };
                if (users.Count() > 0)
                {
                    usersInGroup = users.ToArray();
                }
            }
            return usersInGroup;
        }

        #endregion
    }
}

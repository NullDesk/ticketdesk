using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Security;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Repositories
{
    [Export(typeof(ISecurityRepository))]
    [ExportMetadata("SecurityMode", "AD")]
    public class AdSecurityRepository : SecurityRepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TdAdSecurityRepository"/> class.
        /// </summary>
        /// <param name="roleProvider">The role provider.</param>
        [ImportingConstructor]
        public AdSecurityRepository
            (
                RoleProvider roleProvider,
                [Import("ActiveDirectoryDomain")] Func<string> getActiveDirectoryDomainMethod,
                [Import("ActiveDirectoryUser")] Func<string> getActiveDirectoryUserMethod,
                [Import("ActiveDirectoryUserPassword")] Func<string> getActiveDirectoryUserPasswordMethod
            )
            : base(roleProvider)
        {
            GetActiveDirectoryDomain = getActiveDirectoryDomainMethod;
            GetActiveDirectoryUser = getActiveDirectoryUserMethod;
            GetActiveDirectoryUserPassword = getActiveDirectoryUserPasswordMethod;
        }

        private Func<string> GetActiveDirectoryDomain { get; set; }
        private Func<string> GetActiveDirectoryUser { get; set; }
        private Func<string> GetActiveDirectoryUserPassword { get; set; }

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

            return GetAdUserProperty(userName, "displayName");
        }


        /// <summary>
        /// Gets a property from Active Directory for a user account.
        /// </summary>
        /// <remarks>
        /// Typically used to get display names or email addresses for specific users from AD.
        /// 
        /// These queries perform very poorly due to the nature of AD itself, so once fetched 
        /// the property is stored in a cached collection. Future requests for the same property
        /// will fetch back from the cached collection rather than directly from AD.
        /// </remarks>
        /// <param name="userName">Name of the user.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetAdUserProperty(string userName, string propertyName)
        {
            Dictionary<string, Dictionary<string, string>> userProperties = GetCachedUserProperties();
            string propertyValue = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                if (userName.Contains('\\'))
                {
                    userName = userName.Split('\\')[1];
                }
                //look to see if we already have this user and property value in our collection
                if (userProperties.ContainsKey(userName) && userProperties[userName].ContainsKey(propertyName))
                {
                    propertyValue = userProperties[userName][propertyName];
                }
                else
                {
                    //create the collection for the user if there isn't one already
                    if (!userProperties.ContainsKey(userName))
                    {
                        userProperties.Add(userName, new Dictionary<string, string>());
                    }


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
                                    PropertyValueCollection col = user.Properties[propertyName];
                                    if (col != null && col.Count > 0)
                                    {
                                        propertyValue = col[0].ToString();

                                    }
                                }

                            }

                        }
                    }
                    //add the propertyname/value to the user's entry in the collection even no data 
                    //  was found so that future searches don't attempt to refetch it
                    userProperties[userName].Add(propertyName, propertyValue);
                }
                if (propertyValue.Length < 1) //catch cases where AD property is still empty
                {
                    propertyValue = userName;
                }
            }
            return propertyValue;

        }

        public override bool IsUserInRoleName(string userName, string roleName)
        {
            var members = GetUsersInRole(roleName);
            return members.Any(m => m.Name == userName);
        }

        public override UserInfo[] GetUsersInRole(string roleName)
        {
            string groupName = roleName.Substring(roleName.LastIndexOf('\\') + 1);//just get the group name without any domain portion
            return GetCachedRoleMembers(groupName, GetActiveDirectoryDomain(), GetActiveDirectoryUser(), GetActiveDirectoryUserPassword());
        }

        #region static members

        /// <summary>
        /// Gets the cached collection of user properties.
        /// </summary>
        /// <remarks>
        /// If the collection is not cached, it creates a new, but empty collection and places 
        /// it into cache.
        /// 
        /// The cache is set to expire in 5 minutes to allow occasional refreshes of data from
        /// AD.
        /// </remarks>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, string>> GetCachedUserProperties()
        {
            var oLock = new object();
            lock (oLock)
            {
                ObjectCache cache = MemoryCache.Default;
                var cacheCollection = cache["AdUserPropertyCollection"] as Dictionary<string, Dictionary<string, string>>;

                if (cacheCollection == null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5d);
                    cacheCollection = new Dictionary<string, Dictionary<string, string>>();

                    cache.Set("AdUserPropertyCollection", cacheCollection, policy);
                }
                return cacheCollection;
            }
        }

        /// <summary>
        /// Gets the cached collection of members for a sepcified role.
        /// </summary>
        /// <remarks>
        /// If the collection is not cached, it creates a new, but empty collection and places 
        /// it into cache.
        /// 
        /// The cache is set to expire in 5 minutes to allow occasional refreshes of data from
        /// AD.
        /// </remarks>
        /// <returns></returns>
        private static UserInfo[] GetCachedRoleMembers(string groupName, string activeDirectoryDomain, string activeDirectoryUser, string activeDirectoryUserPassword)
        {
            var oLock = new object();
            lock (oLock)
            {
                string key = GetAdUserGroupCacheKey(groupName);
                ObjectCache cache = MemoryCache.Default;
                var cacheCollection = cache[key] as UserInfo[];

                if (cacheCollection == null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5d);

                    using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, activeDirectoryDomain, activeDirectoryUser, activeDirectoryUserPassword))
                    {
                        using (GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, groupName))
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

                                cacheCollection = x.ToArray();
                            }
                            else
                            {
                                cacheCollection = new UserInfo[0];//zero length array so we don't keep re-attempting to fetch from AD
                            }
                        }
                    }

                    cache.Set(key, cacheCollection, policy);
                }
                return cacheCollection;
            }
        }

        /// <summary>
        /// Gets the appropriate cache key for a particular ad user group name.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        private static string GetAdUserGroupCacheKey(string groupName)
        {
            return string.Format("ActiveDirectoryUserCacheForRole{0}", groupName);
        }

        /// <summary>
        /// Gets the ad user group name from a cache key name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetAdUserGroupFromCacheKey(string key)
        {
            return key.Replace("ActiveDirectoryUserCacheForRole_", string.Empty);
        }

        #endregion

        public override string GetUserEmailAddress(string userName)
        {
            return GetAdUserProperty(userName, "mail");
        }
    }
}
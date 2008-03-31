// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;

namespace TicketDesk.Engine
{
    /// <summary>
    /// Class that manages user and role related operations for TicketDesk.
    /// </summary>
    /// <remarks>
    /// TicketDesk can be configured for either SQL providers or to use Windows 
    /// users and active directory for security.
    /// 
    /// Since these configurations are fundamentally different, this class 
    /// provides a common API that can be used regardless of which configuration
    /// is being used.
    /// </remarks>
    public class SecurityManager
    {
        /// <summary>
        /// Gets the user name formatted for use in the TicketDesk database.
        /// </summary>
        /// <remarks>
        /// For AD users, this removes the domain name from the user name.
        /// </remarks>
        /// <param name="userName">Name of the user to format.</param>
        /// <returns></returns>
        public static string GetFormattedUserName(string userName)
        {
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            if(authenticationSection.Mode == AuthenticationMode.Windows && !string.IsNullOrEmpty(userName) && userName.Contains('\\'))
            {
                userName = userName.Split('\\')[1];
            }
            return userName;
        }



        /// <summary>
        /// Gets the display name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The display name from AD if using windows security, otherwise simply returns the SQL membership user name.</returns>
        public static string GetUserDisplayName(string userName)
        {
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            if(authenticationSection.Mode == AuthenticationMode.Windows && !string.IsNullOrEmpty(userName))
            {
                userName = GetAdUserProperty(userName, "displayName");
            }
            else
            {
                if(!string.IsNullOrEmpty(userName))
                {
                    MembershipUser user = Membership.GetUser(userName);
                    if(user != null && !string.IsNullOrEmpty(user.Comment))
                    {
                        userName = user.Comment;
                    }
                }
            }
            return userName;
        }

        /// <summary>
        /// Gets a collection of all users in role type.
        /// </summary>
        /// <remarks>
        /// Ticket desk has 3 role types that map to either a SQL role provider role or 
        /// an AD group depending on how security is configured. These three role types are
        /// configured in the appsettings section of web.config, and each will specify either
        /// the AD group for that role type, or a role name from the SQL Role provider.
        /// </remarks>
        /// <param name="roleType">
        /// Type of the role (should match one of the appsetting keys from web.config).
        /// </param>
        /// <returns>Users in the role from either the SQL role provider or AD depending on how security is configured.</returns>
        public static User[] GetUsersInRoleType(string roleType)
        {
            User[] users = null;
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            switch(authenticationSection.Mode)
            {
                case AuthenticationMode.Windows:
                    users = GetUsersInRoleWithActiveDirectory(roleType);
                    break;
                case AuthenticationMode.Forms:
                    users = GetUsersInRoleWithSqlRoleProvider(roleType);
                    break;
                default:
                    break;
            }
            return users;
        }


        /// <summary>
        /// Gets the user email address either from the SQL Membership provider or from AD depending 
        /// on how security is configured for the applciation.
        /// </summary>
        /// <param name="userName">Name of the user whose email you wish to fetch.</param>
        /// <returns></returns>
        public static string GetUserEmailAddress(string userName)
        {
            string emailAddress = string.Empty;
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            switch(authenticationSection.Mode)
            {
                case AuthenticationMode.Windows:
                    emailAddress = GetAdUserProperty(userName, "mail");
                    break;
                case AuthenticationMode.Forms:
                    MembershipUser user = Membership.GetUser(userName);
                    if(user != null)
                    {
                        emailAddress = user.Email;
                    }
                    break;
                default:
                    break;
            }

            return emailAddress;
        }

        /// <summary>
        /// Gets the users in a role from the SQL role provider.
        /// </summary>
        /// <param name="roleType">Type of the role.</param>
        /// <returns></returns>
        private static User[] GetUsersInRoleWithSqlRoleProvider(string roleType)
        {
            List<User> users = new List<User>();
            string[] sUsers = Roles.GetUsersInRole(ConfigurationManager.AppSettings[roleType]);
            foreach(string s in sUsers)
            {
                users.Add(new User(s, s));
            }
            return users.ToArray();
        }

        /// <summary>
        /// Callback method used when Cached ad users are removed.
        /// </summary>
        /// <remarks>
        /// This method simply re-fetches the removed collection causing an updated 
        /// version to be re-cached.
        /// </remarks>
        /// <param name="key">The key.</param>
        /// <param name="usersForGroup">The users for group.</param>
        /// <param name="reason">The reason.</param>
        public static void CachedAdUsersForGroupRemovedCallback(string key, object usersForGroup, CacheItemRemovedReason reason)
        {
            GetCachedAdUsersForGroup(GetAdUserGroupFromCacheKey(key));
        }

        /// <summary>
        /// Gets the cached ad users for a group.
        /// </summary>
        /// <remarks>
        /// Users in a group are fetched from AD and stored in cached.
        /// 
        /// The cached item is set to expire in 5 minutes; however, a callback
        /// method is invoked when a collection in cache expires. The callback
        /// will automatically re-fetch the collection from AD and put it into
        /// cache again. This allows group member lists to be refreshed 
        /// occasionally.
        /// </remarks>
        /// <param name="groupName">Name of the group to fetch members for.</param>
        /// <returns></returns>
        private static User[] GetCachedAdUsersForGroup(string groupName)
        {
            string key = GetAdUserGroupCacheKey(groupName);
            if(HttpContext.Current.Cache[key] == null)
            {
                using
                (
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain,
                                            ConfigurationManager.AppSettings["ActiveDirectoryDomain"],
                                            ConfigurationManager.AppSettings["ActiveDirectoryUser"],
                                            ConfigurationManager.AppSettings["ActiveDirectoryUserPassword"])
                )
                {
                    using(GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, groupName))
                    {
                        if(grp != null)
                        {
                            var x = from p in grp.GetMembers(true)
                                    where p.StructuralObjectClass == "user" &&
                                          !string.IsNullOrEmpty(p.UserPrincipalName) &&
                                          !string.IsNullOrEmpty(p.DisplayName) &&
                                          p.UserPrincipalName.Trim() != string.Empty &&
                                          p.DisplayName.Trim() != string.Empty
                                    orderby p.DisplayName
                                    select new User(p.SamAccountName, p.DisplayName);
                            CacheItemRemovedCallback onAdUsersForGroupRemove = new CacheItemRemovedCallback(CachedAdUsersForGroupRemovedCallback);

                            HttpContext.Current.Cache.Insert
                                (
                                    key,
                                    x.ToArray(),
                                    null,
                                    DateTime.Now.AddMinutes(5d),
                                    Cache.NoSlidingExpiration,
                                    System.Web.Caching.CacheItemPriority.Normal,
                                    onAdUsersForGroupRemove
                                );
                        }
                    }
                }
            }
            return (User[])HttpContext.Current.Cache[key];
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

        private static User[] GetUsersInRoleWithActiveDirectory(string roleType)
        {
            string role = ConfigurationManager.AppSettings[roleType];
            string groupName = role.Substring(role.LastIndexOf('\\') + 1);//just get the group name without any domain portion
            return GetCachedAdUsersForGroup(groupName);
        }

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
            if(HttpContext.Current.Cache["AdUserPropertyCollection"] == null)
            {
                Dictionary<string, Dictionary<string, string>> userProperties = new Dictionary<string, Dictionary<string, string>>();
                HttpContext.Current.Cache.Insert
                    (
                        "AdUserPropertyCollection",
                        userProperties,
                        null,
                        DateTime.Now.AddMinutes(5d),
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Normal,
                        null
                    );

            }
            return (Dictionary<string, Dictionary<string, string>>)HttpContext.Current.Cache["AdUserPropertyCollection"];
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
        private static string GetAdUserProperty(string userName, string propertyName)
        {
            Dictionary<string, Dictionary<string, string>> userProperties = GetCachedUserProperties();
            string propertyValue = string.Empty;
            if(userName.Contains('\\'))
            {
                userName = userName.Split('\\')[1];
            }
            //look to see if we already have this user and property value in our collection
            if(userProperties.ContainsKey(userName) && userProperties[userName].ContainsKey(propertyName))
            {
                propertyValue = userProperties[userName][propertyName];
            }
            else
            {
                //create the collection for the user if there isn't one already
                if(!userProperties.ContainsKey(userName))
                {
                    userProperties.Add(userName, new Dictionary<string, string>());
                }


                using
                (
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain,
                                            ConfigurationManager.AppSettings["ActiveDirectoryDomain"],
                                            ConfigurationManager.AppSettings["ActiveDirectoryUser"],
                                            ConfigurationManager.AppSettings["ActiveDirectoryUserPassword"])
                )
                {
                    using(UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userName))
                    {
                        if(userPrincipal != null)
                        {
                            DirectoryEntry user = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
                            if(user != null)
                            {
                                PropertyValueCollection col = user.Properties[propertyName];
                                if(col != null && col.Count > 0)
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
            return propertyValue;
        }

        /// <summary>
        /// Gets a value indicating whether the current user is a member of the help desk staff role type.
        /// </summary>
        /// <value><c>true</c> if the current user is a staff member; otherwise, <c>false</c>.</value>
        public static bool IsStaff
        {
            get
            {
                return Roles.IsUserInRole(ConfigurationManager.AppSettings["HelpDeskStaffRoleName"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is a member of the admin role type.
        /// </summary>
        /// <value><c>true</c> if the current user is an admin member; otherwise, <c>false</c>.</value>
        public static bool IsAdmin
        {
            get
            {
                return Roles.IsUserInRole(ConfigurationManager.AppSettings["AdministrativeRoleName"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is a member of the ticket submitters role type.
        /// </summary>
        /// <value><c>true</c> if the current user is a staff member; otherwise, <c>false</c>.</value>
        public static bool IsTicketSubmitter
        {
            get
            {
                return Roles.IsUserInRole(ConfigurationManager.AppSettings["AdministrativeRoleName"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is a member of eiter the help desk staff or the admin role types.
        /// </summary>
        /// <value><c>true</c> if the current user is a staff member; otherwise, <c>false</c>.</value>
        public static bool IsStaffOrAdmin
        {
            get
            {
                return (IsStaff || IsAdmin);
            }
        }

        /// <summary>
        /// Gets a value indicating whether ticket submitter role type members can set ticket priority.
        /// </summary>
        /// <remarks>
        /// Set via web.config
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if ticket submitters can set ticket priority; otherwise, <c>false</c>.
        /// </value>
        public static bool SubmitterCanEditPriority
        {
            get
            {
                if(ConfigurationManager.AppSettings["AllowSubmitterRoleToEditPriority"] == null)
                {
                    return true;
                }
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowSubmitterRoleToEditPriority"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether ticket submitter role type members can set tags for tickets.
        /// </summary>
        /// <remarks>
        /// Set via web.config
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if ticket submitter role type members can set tags for tickets; otherwise, <c>false</c>.
        /// </value>
        public static bool SubmitterCanEditTags
        {
            get
            {
                if(ConfigurationManager.AppSettings["AllowSubmitterRoleToEditTags"] == null)
                {
                    return true;
                }
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowSubmitterRoleToEditTags"]);

            }
        }
    }
}

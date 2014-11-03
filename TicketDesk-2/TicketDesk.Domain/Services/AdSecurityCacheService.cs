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
using TicketDesk.Domain.Repositories;
using System.ComponentModel.Composition;

namespace TicketDesk.Domain.Services
{
    /// <summary>
    /// Internal service for caching data used by the AD Security System. 
    /// </summary>
    [Export]
    internal class AdSecurityCacheService
    {
        [ImportingConstructor]
        internal AdSecurityCacheService(AdSqlCacheRepository adSqlRepository)
        {
            AdSqlRepository = adSqlRepository;
        }

        [Export("RefreshSecurityCache")]
        [ExportMetadata("SecurityMode", "AD")]
        internal void RefreshAdSqlCaches()
        {
            AdSqlRepository.RefreshSqlCacheFromAd();

            foreach (var key in CurrentCacheKeys)
            {
                MemoryCache.Default.Remove(key);
            }
            CurrentCacheKeys.Clear();
        }

        internal AdSqlCacheRepository AdSqlRepository { get; private set; }


        private static object roleMembersLock = new object();
        /// <summary>
        /// Gets the cached collection of members for a sepcified role.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        /// <remarks>
        /// If the collection is not cached, it creates a new, but empty collection and places
        /// it into cache.
        /// 
        /// The cache is set to expire in 5 minutes to force refreshes from the data store.
        /// </remarks>
        internal UserInfo[] GetRoleMembers(string groupName)
        {
            lock (roleMembersLock)
            {
                string key = GetAdUserGroupCacheKey(groupName);
                ObjectCache cache = MemoryCache.Default;
                var cacheCollection = cache[key] as UserInfo[];

                if (cacheCollection == null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60d);//we set a 60 minute default just so the cache doesn't actually live forever if something is broken with the AD refreshes

                    cacheCollection = AdSqlRepository.GetGroupMembers(groupName);
                    SetCache(cache, key, cacheCollection, policy);

                }
                return cacheCollection;
            }
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
        internal string GetAdUserProperty(string userName, string propertyName)
        {
            Dictionary<string, Dictionary<string, string>> userProperties = GetCachedUserProperties();
            string propertyValue = null;
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

                    propertyValue = AdSqlRepository.GetUserProperty(userName, propertyName);
                    //add the propertyname/value to the user's entry in the collection even no data 
                    //  was found so that future searches don't attempt to refetch it
                    userProperties[userName].Add(propertyName, propertyValue);
                }

            }
            return propertyValue;
        }

        private static List<string> _currentCacheKeys = new List<string>();
        private static List<string> CurrentCacheKeys
        {
            get
            {
                return _currentCacheKeys;
            }
        }

        private void SetCache(ObjectCache cache, string key, object value, CacheItemPolicy policy)
        {
            cache.Add(key, value, policy);
            CurrentCacheKeys.Add(key);
        }


        private static object cachedUserPropertieLock = new object();
       

        /// <summary>
        /// Gets the cached collection of user properties.
        /// </summary>
        /// <returns>The collection containing user properties</returns>
        /// <remarks>
        /// If the collection is not cached, it creates a new, but empty collection and places
        /// it into memory.
        /// The cache is set to expire in 5 minutes to force refreshes from the data store.
        /// </remarks>
        private Dictionary<string, Dictionary<string, string>> GetCachedUserProperties()
        {
            lock (cachedUserPropertieLock)
            {
                ObjectCache cache = MemoryCache.Default;
                var cacheCollection = cache["AdUserPropertyCollection"] as Dictionary<string, Dictionary<string, string>>;

                if (cacheCollection == null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60d);//we set a 60 minute default just so the cache doesn't actually live forever if something is broken with the AD refreshes
                    cacheCollection = new Dictionary<string, Dictionary<string, string>>();
                    SetCache(cache, "AdUserPropertyCollection", cacheCollection, policy);
                }
                return cacheCollection;
            }
        }

        /// <summary>
        /// Gets the appropriate cache key for a particular ad user group name.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        private string GetAdUserGroupCacheKey(string groupName)
        {
            return string.Format("ActiveDirectoryUserCacheForRole{0}", groupName);
        }

        /// <summary>
        /// Gets the ad user group name from a cache key name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string GetAdUserGroupFromCacheKey(string key)
        {
            return key.Replace("ActiveDirectoryUserCacheForRole_", string.Empty);
        }

        public void InitializeCacheManagement()
        {
            throw new NotImplementedException();
        }
    }
}

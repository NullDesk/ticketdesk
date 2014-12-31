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
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public class UserDisplayInfoCache : List<UserDisplayInfo>
    {
        private TicketDeskUserManager Manager { get; set; }
        public UserDisplayInfoCache(TicketDeskUserManager manager)
        {
            Manager = manager;
        }

        public void ClearCache()
        {
            MemoryCache.Default.Remove("user-info-cache");
        }

        public UserDisplayInfo GetUserInfo(string userId)
        {
            return GetCachedUserInfo(Manager).FirstOrDefault(u => u.Id == userId);
        }

        private static IEnumerable<UserDisplayInfo> GetCachedUserInfo(UserManager<TicketDeskUser, string> manager)
        {
            const string key = "user-info-cache";
            var cache = MemoryCache.Default;
            var cacheCollection = cache[key] as IEnumerable<UserDisplayInfo>;

            if (cacheCollection == null)
            {
                var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15d) };

                cacheCollection = manager.Users.Select(u => new UserDisplayInfo
                {
                    Id = u.Id,
                    Email = u.Email,
                    DisplayName = u.DisplayName
                }).ToList();
                cache.Add(key, cacheCollection, policy);
            }
            return cacheCollection;

        }
    }

    public class UserDisplayInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public static UserDisplayInfo GetUserInfo(string userId)
        {
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.InfoCache.GetUserInfo(userId) ?? new UserDisplayInfo();
        }

    }
}

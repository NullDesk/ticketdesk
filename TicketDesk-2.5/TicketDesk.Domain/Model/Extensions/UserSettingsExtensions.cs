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

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    public static class UserSettingsExtensions
    {
        /// <summary>
        /// Gets the user list setting by name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>UserTicketListSetting.</returns>
        public static UserTicketListSetting GetUserListSettingByName(this DbSet<UserSetting> settings, string listName, string userId)
        {
            return GetUserSettings(settings, userId).ListSettings.FirstOrDefault(us => us.ListName == listName);
        }

        /// <summary>
        /// Gets all user list settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>ICollection{UserTicketListSetting}.</returns>
        public static ICollection<UserTicketListSetting> GetAllUserListSettings(this DbSet<UserSetting> settings,
            string userId)
        {
            return GetUserSettings(settings, userId).ListSettings;

        }

        /// <summary>
        /// Gets the user settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>UserSetting.</returns>
        private static UserSetting GetUserSettings(DbSet<UserSetting> settings, string userId)
        {
            return (settings.FirstOrDefault(us => us.UserId == userId) ??
                    UserSetting.GetDefaultSettingsForUser(userId));
        }
    }
}

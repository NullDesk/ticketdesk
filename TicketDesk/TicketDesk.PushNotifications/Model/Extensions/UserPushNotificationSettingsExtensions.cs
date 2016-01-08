// TicketDesk - Attribution notice
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

using System.Data.Entity;
using System.Linq;

namespace TicketDesk.PushNotifications.Model.Extensions
{
    public static class UserPushNotificationSettingsExtensions
    {
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>UserSetting.</returns>
        public static SubscriberNotificationSetting GetSettingsForUser(this DbSet<SubscriberNotificationSetting> settings, string userId)
        {
            var usetting = settings.FirstOrDefault(us => us.SubscriberId == userId) ?? 
                new SubscriberNotificationSetting();//when no settings for user in db, return default settings
            return usetting;
        }
    }
}

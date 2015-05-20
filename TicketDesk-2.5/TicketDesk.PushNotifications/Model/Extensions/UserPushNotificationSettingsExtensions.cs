using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

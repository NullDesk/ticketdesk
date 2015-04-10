using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common
{
    public class SubscriberPushNotificationSettingsManager
    {
        private TdPushNotificationContext NotificationContext { get; set; }
        public SubscriberPushNotificationSettingsManager(TdPushNotificationContext notificationContext)
        {
            NotificationContext = notificationContext;
        }

        public async Task<SubscriberPushNotificationSetting> GetSettingsForSubscriber(string subscriberId)
        {
            return await NotificationContext.SubscriberPushNotificationSettings.FindAsync(subscriberId);
        }

        public void AddSettingsForSubscriber(SubscriberPushNotificationSetting settings)
        {
            NotificationContext.SubscriberPushNotificationSettings.Add(settings);
        }
    }
}

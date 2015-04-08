using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.PushNotifications.Common.Model
{
    public class PushNotificationEventInfo
    {
        public int TicketId { get; set; }

        public string SubscriberId { get; set; }

        public int EventId { get; set; }

        public bool CancelNotification { get; set; }

        internal PushNotificationItem ToPushNotificationItem(ApplicationPushNotificationSetting appSettings, SubscriberPushNotificationSetting userSettings)
        {
            return new PushNotificationItem()
            {
                TicketId = TicketId,
                SubscriberId = SubscriberId,
                DeliveryStatus =
                            userSettings.IsEnabled
                                ? (CancelNotification)? PushNotificationItemStatus.Canceled: PushNotificationItemStatus.Scheduled
                                : PushNotificationItemStatus.Disabled,
                RetryCount = 0,
                CreatedDate = DateTimeOffset.Now,
                ScheduledSendDate = CancelNotification? null : GetSendDate(appSettings, userSettings),
                TicketEvents = CancelNotification ? new Collection<int>() : new Collection<int>(new[] { EventId }),
                CanceledEvents = CancelNotification? new Collection<int>(new[] { EventId }) : new Collection<int>()
            };
        }


        private static DateTimeOffset? GetSendDate(ApplicationPushNotificationSetting appSettings, SubscriberPushNotificationSetting userNoteSettings)
        {
            DateTimeOffset? send = null;
            if (userNoteSettings.IsEnabled)
            {
                var addMinutes = appSettings.AntiNoiseSettings.IsConsolidationEnabled
                    ? appSettings.AntiNoiseSettings.InitialConsolidationDelayMinutes
                    : 0;
                send = DateTime.Now.AddMinutes(addMinutes);
            }
            return send;

        }
    }
}

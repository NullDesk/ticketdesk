using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.PushNotifications.Model
{
    public class NewTicketPushNotificationInfo
    {
        public int TicketId { get; set; }


        public string MessageContent { get; set; }

        internal IEnumerable<PushNotificationItem> ToPushNotificationItems(
            SubscriberNotificationSetting userSettings
            )
        {
            var now = DateTimeOffset.Now;
            return userSettings.PushNotificationDestinations.Select(dest =>
                new PushNotificationItem()
                {
                    ContentSourceId = TicketId,
                    ContentSourceType = "new ticket",
                    SubscriberId = "new ticket broadcast",
                    Destination = dest,
                    DestinationId = dest.DestinationId,
                    DeliveryStatus = userSettings.IsEnabled
                        ? PushNotificationItemStatus.Scheduled
                        : PushNotificationItemStatus.Disabled,
                    RetryCount = 0,
                    CreatedDate = now,
                    ScheduledSendDate = now,
                    MessageContent = MessageContent
                }
            ).ToList();

        }
    }
}

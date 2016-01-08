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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TicketDesk.PushNotifications.Model
{
    public class TicketPushNotificationEventInfo
    {
        public int TicketId { get; set; }

        public string SubscriberId { get; set; }

        public int EventId { get; set; }

        public bool CancelNotification { get; set; }

        public string MessageContent { get; set; } 

        internal IEnumerable<TicketPushNotificationItem> ToPushNotificationItems(
            ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userSettings)
        {
            var now = DateTimeOffset.Now;
            return userSettings.PushNotificationDestinations.Select(dest =>
            new TicketPushNotificationItem()
                {
                    PushNotificationItem = new PushNotificationItem
                    {
                        ContentSourceId = TicketId,
                        ContentSourceType = "ticket",
                        SubscriberId = SubscriberId,
                        Destination = dest,
                        DestinationId = dest.DestinationId,
                        DeliveryStatus = userSettings.IsEnabled ? (CancelNotification) ? PushNotificationItemStatus.Canceled : PushNotificationItemStatus.Scheduled : PushNotificationItemStatus.Disabled,
                        RetryCount = 0,
                        CreatedDate = now,
                        ScheduledSendDate = CancelNotification ? null : GetSendDate(now, appSettings, userSettings),
                        MessageContent = MessageContent

                    },
                    TicketEvents = CancelNotification ? new Collection<int>() : new Collection<int>(new[] { EventId }),
                    CanceledEvents = CancelNotification ? new Collection<int>(new[] { EventId }) : new Collection<int>()
                }
            ).ToList();
        }


        private static DateTimeOffset? GetSendDate(DateTimeOffset now, ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userNoteSettings)
        {
            DateTimeOffset? send = null;
            if (userNoteSettings.IsEnabled)
            {
                var addMinutes = appSettings.AntiNoiseSettings.IsConsolidationEnabled
                    ? appSettings.AntiNoiseSettings.InitialConsolidationDelayMinutes
                    : 0;
                send = now.AddMinutes(addMinutes);
            }
            return send;

        }
    }
}

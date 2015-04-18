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
using System.Collections.ObjectModel;

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

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
using System.Web.Mvc;
using TicketDesk.Notifications.Common;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventNotificationExtensions
    {
        public static IEnumerable<NotificationItem> ToNotificationItemItems(this IEnumerable<TicketEventNotification> notifications)
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            if (context.TicketDeskSettings.PushNotificationSettings.IsEnabled)
            {
                return notifications.Select(note =>
                {
                    var userSettings = context.UserSettings.GetUserSetting(note.SubscriberId);

                    return new NotificationItem
                    {
                        TicketId = note.TicketId,
                        SubscriberId = note.SubscriberId,
                        DeliveryStatus =
                            userSettings.PushNotificationSettings.IsEnabled
                                ? NotificationItemStatus.Scheduled
                                : NotificationItemStatus.Disabled,
                        RetryCount = 0,
                        CreatedDate = DateTimeOffset.Now,
                        ScheduledSendDate =
                            userSettings.PushNotificationSettings.IsEnabled
                                ? DateTime.Now.AddMinutes(5)
                                : (DateTimeOffset?) null,
                        IncludedTicketEvents = new[] {note.EventId}
                    };
                });
            }
            return null;
        }
    }
}
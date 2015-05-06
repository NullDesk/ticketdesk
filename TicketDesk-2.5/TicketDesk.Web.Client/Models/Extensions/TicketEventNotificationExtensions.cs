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

using System.Collections.Generic;
using System.Linq;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventNotificationExtensions
    {
        public static IEnumerable<PushNotificationEventInfo> ToNotificationEventInfoCollection(this IEnumerable<TicketEventNotification> eventNotifications, bool subscriberExclude)
        {
            return eventNotifications.Select(note => new PushNotificationEventInfo()
            {
                TicketId = note.TicketId,
                SubscriberId = note.SubscriberId,
                EventId = note.EventId,
                CancelNotification = subscriberExclude && note.IsRead
            });

        }

       
    }
}
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

using System.Data.Entity;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventNotificationExtensions
    {
        public static void AddNotificationsForEvent(this IDbSet<TicketEventNotification> set,
            TicketEvent ticketEvent)
        {
            //PushNotificationPending could be set baed on subscriber's preferences in settings
            //  In this case though, I'm somewhat concerned about the number of queries required to setup
            //  notifications. Adding more lazy loads to pull in subscriber preferences from json serialized
            //  settings could be quite cumbersome. Instead, we'll assume that the push notifier will decide 
            //  if it should actually send the notifications or not.
            foreach (var subscriber in ticketEvent.Ticket.TicketSubscribers)
            {
                if (ticketEvent.EventBy != subscriber.SubscriberId)
                {
                    set.Add(new TicketEventNotification
                    {
                        IsNew = true,
                        IsRead = false,
                        PushNotificationPending = true,
                        SubscriberId = subscriber.SubscriberId,
                    });
                }
            }           
        }
    }
}

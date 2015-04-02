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

namespace TicketDesk.Notifications.Common
{
    public enum NotificationItemStatus
    {
        Scheduled,
        Sending,
        Sent,
        Retrying,
        Failed,
        Canceled,
        Disabled

    }
    public class NotificationItem
    {
        public int TicketId { get; set; }
        public string SubscriberId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ScheduledSendDate { get; set; }
        public NotificationItemStatus DeliveryStatus { get; set; }
        public int RetryCount { get; set; }
        public int[] IncludedTicketEvents { get; set; }
        
    }
}

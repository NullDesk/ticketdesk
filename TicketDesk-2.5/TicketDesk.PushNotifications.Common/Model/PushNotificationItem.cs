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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TicketDesk.PushNotifications.Common.Model
{
    public enum PushNotificationItemStatus
    {
        Scheduled,
        Sending,
        Sent,
        Retrying,
        Failed,
        Canceled,
        Disabled

    }
    public class PushNotificationItem
    {
        [Key]
        [Column(Order = 0)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string SubscriberId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ScheduledSendDate { get; set; }
        
        public PushNotificationItemStatus DeliveryStatus { get; set; }
        
        public int RetryCount { get; set; }
        
        public string TicketEventsList { get; set; }

        [NotMapped]
        public int[] TicketEvents
        {
            get
            {
                return Array.ConvertAll(TicketEventsList.Split(';'), int.Parse);
            }
            set
            {
                TicketEventsList = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }
       
    }

  
}

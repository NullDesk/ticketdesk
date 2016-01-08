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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TicketDesk.PushNotifications.Model
{
    [Table("TicketPushNotificationItems", Schema = "notifications")]
    public class TicketPushNotificationItem
    {
        [Key]
        public int PushNotificationItemId { get; set; }

        [ForeignKey("PushNotificationItemId")]
        public virtual PushNotificationItem PushNotificationItem { get; set; }

        public string TicketEventsList
        {
            get { return string.Join(",", TicketEvents.Select(p => p.ToString()).ToArray()); }
            set
            {
                TicketEvents = string.IsNullOrEmpty(value) ?
                    new List<int>() :
                    new List<int>(Array.ConvertAll(value.Split(','), int.Parse));
            }
        }

        public string CanceledEventsList
        {
            get { return string.Join(",", CanceledEvents.Select(p => p.ToString()).ToArray()); }
            set
            {
                CanceledEvents = string.IsNullOrEmpty(value) ?
                    new List<int>() :
                    new List<int>(Array.ConvertAll(value.Split(','), int.Parse));
            }
        }

        [NotMapped]
        public ICollection<int> TicketEvents{get; set;}

        [NotMapped]
        public ICollection<int> CanceledEvents { get; set; }
        

        public void AddNewEvent(TicketPushNotificationEventInfo eventInfo, ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userSetting)
        {
            //no matter what, update to the current message content
            PushNotificationItem.MessageContent = eventInfo.MessageContent;

            if (eventInfo.CancelNotification)
            {
                //remove event
                TicketEvents.Remove(eventInfo.EventId);
                CanceledEvents.Add(eventInfo.EventId);
                if (!TicketEvents.Any())//no events left, scrap entire notification
                {
                    PushNotificationItem.DeliveryStatus = PushNotificationItemStatus.Canceled;
                    PushNotificationItem.ScheduledSendDate = null;
                }
            }
            else
            {
                //add this event
                TicketEvents.Add(eventInfo.EventId);
                //kick the schedule out if consolidation enabled
                PushNotificationItem.ScheduledSendDate = PushNotificationItem.GetSendDate(appSettings, userSetting);
            }
        }
    }
}

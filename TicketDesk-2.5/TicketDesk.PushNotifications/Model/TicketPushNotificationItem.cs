using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

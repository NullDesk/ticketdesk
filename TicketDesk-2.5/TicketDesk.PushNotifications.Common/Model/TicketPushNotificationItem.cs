using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.PushNotifications.Common.Model
{
    [Table("TicketPushNotificationItems", Schema = "notifications")]
    public class TicketPushNotificationItem
    {
        [Key]
        public int PushNotificationItemId { get; set; }

        [ForeignKey("PushNotificationItemId")]
        public virtual PushNotificationItem PushNotificationItem { get; set; }

        public string TicketEventsList { get; set; }

        public string CanceledEventsList { get; set; }

        [NotMapped]
        public Collection<int> TicketEvents
        {
            get
            {
                return new Collection<int>(Array.ConvertAll(TicketEventsList.Split(';'), int.Parse));
            }
            set
            {
                TicketEventsList = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }

        [NotMapped]
        public Collection<int> CanceledEvents
        {
            get
            {
                return new Collection<int>(Array.ConvertAll(CanceledEventsList.Split(';'), int.Parse));
            }
            set
            {
                CanceledEventsList = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }

        public void AddNewEvent(TicketPushNotificationEventInfo eventInfo, ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userSetting)
        {
            if (eventInfo.CancelNotification)
            {
                TicketEvents.Remove(eventInfo.EventId);
                CanceledEvents.Add(eventInfo.EventId);
                if (!TicketEvents.Any())
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

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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TicketDesk.PushNotifications.Common.Model
{

    [Table("PushNotificationItems", Schema = "notifications")]
    public class PushNotificationItem
    {
        [Key]
        [Column(Order = 0)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string SubscriberId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string DestinationAddress { get; set; }

        [Key]
        [Column(Order = 3)]
        public string DestinationType { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ScheduledSendDate { get; set; }

        public PushNotificationItemStatus DeliveryStatus { get; set; }

        public int RetryCount { get; set; }

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

        public void AddNewEvent(PushNotificationEventInfo eventInfo, ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userSetting)
        {
            if (eventInfo.CancelNotification)
            {
                TicketEvents.Remove(eventInfo.EventId);
                CanceledEvents.Add(eventInfo.EventId);
                if (!TicketEvents.Any())
                {
                    DeliveryStatus = PushNotificationItemStatus.Canceled;
                    ScheduledSendDate = null;
                }
            }
            else
            {
                //add this event
                TicketEvents.Add(eventInfo.EventId);
                //kick the schedule out if consolidation enabled
                ScheduledSendDate = GetSendDate(appSettings, userSetting);
            }
        }

        private DateTimeOffset? GetSendDate(ApplicationPushNotificationSetting appSettings, SubscriberNotificationSetting userNoteSettings)
        {
            var send = ScheduledSendDate;//we'll leave this alone if consolidation isn't used

            if (userNoteSettings.IsEnabled && appSettings.AntiNoiseSettings.IsConsolidationEnabled && ScheduledSendDate.HasValue)
            {
                var now = DateTime.Now;

                var currentDelayMinutes = (now - CreatedDate).Minutes;

                var maxDelayMinutes = appSettings.AntiNoiseSettings.MaxConsolidationDelayMinutes;

                var delay = appSettings.AntiNoiseSettings.InitialConsolidationDelayMinutes;

                //ensure we don't bump thing over the max wait
                if (delay + currentDelayMinutes > maxDelayMinutes)
                {
                    delay = maxDelayMinutes - currentDelayMinutes;
                }

                send = now.AddMinutes(delay);
            }
            return send;

        }
    }


}

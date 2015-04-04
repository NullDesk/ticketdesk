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
using TicketDesk.Domain.Annotations;
using TicketDesk.Domain.Localization;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TicketEvent
    {
        public TicketEvent()
        {
            TicketEventNotifications = new HashSet<TicketEventNotification>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; [UsedImplicitly] set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [StringLength(500)]
        public string EventDescription { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        public bool IsHtml { get; set; }

        [Required]
        [StringLength(256)]
        public string EventBy { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset EventDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; [UsedImplicitly] set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ICollection<TicketEventNotification> TicketEventNotifications { get; set; }


        /// <summary>
        /// Creates the activity event.
        /// </summary>
        /// <param name="eventByUserId">The event by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="newPriority">The new priority.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>TicketEvent.</returns>
        public static TicketEvent CreateActivityEvent(
        string eventByUserId,
        TicketActivity activity,
        string comment,
        string newPriority,
        string userName)
        {
            var tc = new TicketEvent
            {
                Comment = comment,
                EventBy = eventByUserId,
                EventDate = DateTime.Now,
                EventDescription = TicketTextUtility.GetTicketEventDescription(activity, newPriority, userName),
                IsHtml = false
            };
            return tc;
        }

        /// <summary>
        /// Creates the event notifications for each ticket subscriber and adds them to the TicketEventNotifications collection.
        /// </summary>
        public void CreateSubscriberEventNotifications()
        {
            //PushNotificationPending could be set baed on subscriber's preferences in settings
            //  In this case though, I'm somewhat concerned about the number of queries required to setup
            //  notifications. Adding more lazy loads to pull in subscriber preferences from json serialized
            //  settings could be quite cumbersome. Instead, we'll assume that the push notifier will decide 
            //  if it should actually send the notifications or not.
            foreach (var subscriber in Ticket.TicketSubscribers)
            {
                //TODO: need to base this if exclusion on the ExcludeSubscriberEvents setting in application anti-noise settings
                if (EventBy != subscriber.SubscriberId)
                {
                    TicketEventNotifications.Add(
                        new TicketEventNotification
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

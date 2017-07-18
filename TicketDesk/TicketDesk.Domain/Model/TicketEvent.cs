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
using TicketDesk.Domain.Localization;
using TicketDesk.Localization;

namespace TicketDesk.Domain.Model
{
    public class TicketEvent
    {
        public TicketEvent()
        {
            TicketEventNotifications = new HashSet<TicketEventNotification>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [StringLength(500, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        public string EventDescription { get; set; }


        public string Comment { get; set; }

        public bool IsHtml { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(256, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        public string EventBy { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset EventDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ICollection<TicketEventNotification> TicketEventNotifications { get; set; }

        public TicketActivity ForActivity { get; set; }


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
                ForActivity = activity,
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
            
            foreach (var subscriber in Ticket.TicketSubscribers)
            {
                var isSubscriberEvent = EventBy == subscriber.SubscriberId;

                TicketEventNotifications.Add(
                    new TicketEventNotification
                    {
                        IsNew = !isSubscriberEvent,
                        IsRead = isSubscriberEvent,
                        SubscriberId = subscriber.SubscriberId,
                    });

            }
        }
    }
}

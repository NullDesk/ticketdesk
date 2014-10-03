// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(INotificationQueuingService))]
    public class NotificationQueuingService : TicketDesk.Domain.Services.INotificationQueuingService
    {
        [ImportingConstructor]
        public NotificationQueuingService
        (
            ISecurityService security,
            [Import("EmailNotificationsInitialDelayMinutes")] Func<double> getNotificationsInitialDelayMethod,
            [Import("HelpDeskBroadcastNotificationsEnabled")] Func<bool> getHelpDeskBroadcastNotificationsEnabled 
        )
        {
            GetNotificationsInitialDelay = getNotificationsInitialDelayMethod;
            GetHelpDeskBroadcastNotificationsEnabled = getHelpDeskBroadcastNotificationsEnabled;
            Security = security;
        }

        public ISecurityService Security { get; private set; }
        public Func<bool> GetNotificationsEnabled { get; private set; }
        public Func<bool> GetHelpDeskBroadcastNotificationsEnabled { get; private set; } 
        public Func<double> GetNotificationsInitialDelay { get; private set; }

        /// <summary>
        /// Adds the ticket event notifications for the supplied comment.
        /// </summary>
        /// <param name="comment">The comment for which notifications should be added.</param>
        /// <param name="isGiveUpTicket">if set to <c>true</c> is a give up ticket action.</param>
        /// <param name="subscribers">The subscribers.</param>
        public void AddTicketEventNotifications(TicketComment comment, bool isNewOrGiveUpTicket, string[] subscribers)
        {


            Dictionary<string, string> userReasons = GetNotificationUsersForComment(isNewOrGiveUpTicket, subscribers);
            var newNotes = CreateNotesForUsers(userReasons, comment);
            ScheduleNoteDeliveries(newNotes);

            foreach (var note in newNotes)
            {
                comment.TicketEventNotifications.Add(note);
            }

        }

        private void ScheduleNoteDeliveries(List<TicketEventNotification> newNotes)
        {
            foreach (var note in newNotes)
            {
                note.DeliveryAttempts = 0;

                if (note.NotifyEmail == "invalid")//notes with invalid email still added to table, but squash schedule.
                {
                    note.Status = "invalid";
                    note.NextDeliveryAttemptDate = null;
                    note.LastDeliveryAttemptDate = DateTime.Now;
                }
                else
                {
                    note.Status = "queued";
                    var now = DateTime.Now;

                    if (note.NotifyUserReason == "HelpDesk")// for non-broadcasts to helpdesk schedule on the delay 
                    {
                        if (GetHelpDeskBroadcastNotificationsEnabled())
                        {
                            note.NextDeliveryAttemptDate = now;
                        }
                        else
                        {
                            note.NextDeliveryAttemptDate = null;
                            note.Status = "suppressed";
                            note.LastDeliveryAttemptDate = now;
                        }
                        
                        
                    }
                    else
                    {
                        note.NextDeliveryAttemptDate = now.AddMinutes(GetNotificationsInitialDelay());
                    }
                }
            }
        }

        private List<TicketEventNotification> CreateNotesForUsers(Dictionary<string, string> userReasons, TicketComment comment)
        {
            List<TicketEventNotification> newNotes = new List<TicketEventNotification>();
            var dt = DateTime.Now;
            foreach (var userReason in userReasons)
            {
                var note = new TicketEventNotification();
                note.CreatedDate = dt;
                note.EventGeneratedByUser = comment.CommentedBy;

                //validate email address, if not valid we'll queue the note but not bother sending it (and having to go through the retries)
                bool emailValid = false;
                string email = Security.GetUserEmailAddress(userReason.Key);
                var rxv = new RegexStringValidator(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                try
                {
                    rxv.Validate(email);
                    emailValid = true;
                }
                catch { }

                note.NotifyEmail = (emailValid && !string.IsNullOrEmpty(email)) ? email : "invalid";
                note.NotifyUser = userReason.Key;
                note.NotifyUserDisplayName = Security.GetUserDisplayName(userReason.Key);
                note.NotifyUserReason = userReason.Value;
                newNotes.Add(note);
            }
            return newNotes;
        }

        private Dictionary<string, string> GetNotificationUsersForComment(bool isNewOrGiveUpTicket, string[] subscribers)
        {
            Dictionary<string, string> usersToAdd = new Dictionary<string, string>();

            foreach (string subscriber in subscribers)
            {
                if (!usersToAdd.ContainsKey(subscriber))
                {
                    usersToAdd.Add(subscriber, "Subscriber");
                }
            }
            //only add help desk for brand new tickets (they should have exactly 1 comment when new)
            //  or tickets that have been given up on (was assigned but is not anymore).
            if (isNewOrGiveUpTicket)
            {
                string[] staffers = Security.GetTdStaffUsers().Select(a => a.Name).ToArray();
                foreach (string staff in staffers)
                {
                    if (!usersToAdd.ContainsKey(staff))
                    {
                        usersToAdd.Add(staff, "HelpDesk");
                    }
                }
            }
            return usersToAdd;
        }

    }
}

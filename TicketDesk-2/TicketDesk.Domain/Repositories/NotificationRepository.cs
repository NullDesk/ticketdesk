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
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Repositories
{
    [Export(typeof(INotificationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NotificationRepository : INotificationRepository
    {
        private TicketDeskEntities ctx = new TicketDeskEntities();

        public IOrderedQueryable<TicketEventNotification> GetQueuedNotifications()
        {
            return ctx.TicketEventNotifications.Where(n => n.NextDeliveryAttemptDate.HasValue).OrderBy(n => n.NextDeliveryAttemptDate);
            
        }


        /// <summary>
        /// Updates the ticket event notifications.
        /// </summary>
        /// <remarks>
        /// In the LINQ to SQL implementation we don't need the ticket notification entities passed as a parameter, but we pass them anyway as other repositories may not work the same way
        /// </remarks>
        /// <param name="notifications">The ticket event notifiactions to update.</param>
        /// <returns></returns>
        public bool UpdateNotifications(IEnumerable<TicketEventNotification> notifications)
        {
            //In the L2S implementation we don't have to save the notes passed in, we just commit the context.
            //  other implemenations may need to do it differently.
            ctx.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            
            return true;
        }


        /// <summary>
        /// Ensures a valid next delivery date for any newly queud notifications when a notification has been rescheduled.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <param name="notifyUser">The notify user.</param>
        /// <param name="lastSentCommentId">The last sent comment id.</param>
        /// <param name="nextDeliveryAttemptDate">The next delivery attempt date.</param>
        /// <remarks>
        /// While the system is processing ticket emails, new notifications could get queued up.
        /// This method should be called if the sending process has to re-schedule a notifiaciton 
        /// for a later re-try attempt. This will keep the notification that was being processed from
        /// having it's next delivery date kicked out further than newer comments that have queued up.
        /// It should look for any new notifications that are queued for the same user and ticket, 
        /// and ensure that their next-delivery date remains greater than the last one that was processed 
        /// and rescheduled.
        /// </remarks>
        public void EnsureValidNextDeliveryDateForNewlyQueudNotifications(int ticketId, string notifyUser, int lastSentCommentId, DateTime nextDeliveryAttemptDate)
        {

            using (var ctxNew = new TicketDeskEntities())
            {
                //the class's standard context will have cached data from any past reads. Do this on new context
                var newNotes = from tn in ctxNew.TicketEventNotifications
                               where tn.TicketId == ticketId && tn.NotifyUser == notifyUser && tn.CommentId > lastSentCommentId && tn.NextDeliveryAttemptDate <= nextDeliveryAttemptDate
                               select tn;
                if (newNotes.Count() > 0)
                {
                    int advanceSeconds = 1;
                    foreach (var newNote in newNotes)
                    {
                        newNote.NextDeliveryAttemptDate = nextDeliveryAttemptDate.AddSeconds(advanceSeconds);
                        advanceSeconds++;
                    }
                    ctxNew.SaveChanges();
                }
            }
        }
    }
}

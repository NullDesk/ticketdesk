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
using TicketDesk.Domain.Repositories;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(INotificationSendingService))]
    public class NotificationSendingService : INotificationSendingService
    {
        [ImportingConstructor]
        public NotificationSendingService
        (
            [Import("EmailMaxConsolidationWaitMinutes")] Func<double> getEmailMaxConsolidationWaitMinutesMethod,
            [Import("EmailResendDelayMinutes")]  Func<int> getEmailResendDelayMinutesMethod,
            [Import("EmailMaxDeliveryAttempts")]  Func<int> getEmailMaxDeliveryAttemptsMethod,
            [Import("FromEmailDisplayName")] Func<string> getFromEmailDisplayNameMethod,
            [Import("FromEmailAddress")] Func<string> getFromEmailAddressMethod,
            [Import("BlindCopyToEmailAddress")] Func<string> getBlindCopyToEmailAddressMethod,
            [Import("EmailServiceName")]Func<string> getEmailServiceNameMethod,
            [Import("TicketNotificationHtmlEmailContent")]  Func<TicketEventNotification,  int, string> getTicketNotificationHtmlEmailContentMethod,
            [Import("TicketNotificationTextEmailContent")]  Func<TicketEventNotification, int, string> getTicketNotificationTextEmailContentMethod,
            [ImportMany(typeof(IEmailHandlerService))]IEmailHandlerService[] emailHandlers,
            INotificationRepository notificationRepository
        )
        {
            NotificationRepository = notificationRepository;
            GetEmailMaxConsolidationWaitMinutes = getEmailMaxConsolidationWaitMinutesMethod;
            GetEmailResendDelayMinutes = getEmailResendDelayMinutesMethod;
            GetEmailMaxDeliveryAttempts = getEmailMaxDeliveryAttemptsMethod;
           
            GetFromEmailDisplayName = getFromEmailDisplayNameMethod;
            GetFromEmailAddress = getFromEmailAddressMethod;
            GetBlindCopyToEmailAddress = getBlindCopyToEmailAddressMethod;
            GetTicketNotificationHtmlEmailContent = getTicketNotificationHtmlEmailContentMethod;
            GetTicketNotificationTextEmailContent = getTicketNotificationTextEmailContentMethod;
            GetEmailServiceName = getEmailServiceNameMethod;
            EmailHandlers = emailHandlers;
        }

        public Func<double> GetEmailMaxConsolidationWaitMinutes { get; private set; }
        public Func<int> GetEmailResendDelayMinutes { get; private set; }
        public Func<int> GetEmailMaxDeliveryAttempts { get; private set; }
       
        public Func<string> GetFromEmailDisplayName { get; private set; }
        public Func<string> GetFromEmailAddress { get; private set; }
        public Func<string> GetBlindCopyToEmailAddress { get; private set; }

        public Func<TicketEventNotification, int, string> GetTicketNotificationHtmlEmailContent { get; private set; }
        public Func<TicketEventNotification, int, string> GetTicketNotificationTextEmailContent { get; private set; }

        Func<string> GetEmailServiceName { get; set; }

        public INotificationRepository NotificationRepository { get; private set; }

        public IEmailHandlerService[] EmailHandlers { get; private set; }
        public IEmailHandlerService EmailHandler
        {
            get
            {
                IEmailHandlerService svcs = null;
                var sec = GetEmailServiceName();
                foreach (var s in EmailHandlers)
                {
                    foreach (Attribute attribute in s.GetType().GetCustomAttributes(false))
                    {
                        ExportMetadataAttribute emAttribute = attribute as ExportMetadataAttribute;
                        if (emAttribute != null && emAttribute.Name == "EmailServiceName" && emAttribute.Value.Equals(sec))
                        {
                            svcs = s;
                            break;
                        }
                    }
                    if (svcs != null)
                    {
                        break;
                    }
                }
                return svcs;
            }
        }


        //TODO: These may be best suited to being static rather than instance based... depends on invocation and testability

        private static object processWaitingTicketEventNotificationsLock = new object();
        public void ProcessWaitingTicketEventNotifications()
        {
            lock (processWaitingTicketEventNotificationsLock)
            {
                var now = DateTime.Now;


                var queuedNotes = (from n in NotificationRepository.GetQueuedNotifications()
                                  group n by new { n.NotifyUser, n.TicketId } into userTicketNotes
                                  select new { userTicketNotes.Key.NotifyUser, userTicketNotes.Key.TicketId, userTicketNotes }).ToList();
                var alteredNotes = new List<TicketEventNotification>();
                try
                {
                    foreach (var ut in queuedNotes)
                    {
                        var consolidationWait = GetEmailMaxConsolidationWaitMinutes();

                        if (ut.userTicketNotes.Count(n => n.NextDeliveryAttemptDate <= now) > 0)// at least 1 note ready, process
                        {
                            var minNextDeliveryTime = ut.userTicketNotes.Min(n => n.NextDeliveryAttemptDate);//gets the min next delivery date for pending notes
                            var minPending = ut.userTicketNotes.SingleOrDefault
                                (
                                    note => (note.CommentId == (ut.userTicketNotes.Min(n => n.CommentId)))
                                );// user's oldest pending comment (by comment ID just in case next delivery dates get out of relative order by accident)


                            var maxPendingDeliveryTime = ut.userTicketNotes.Max(n => n.NextDeliveryAttemptDate);// gets the max next delivery date for pending notes
                            var maxPending = ut.userTicketNotes.SingleOrDefault
                                (
                                    note =>
                                        (note.CommentId == (ut.userTicketNotes.Max(n => n.CommentId)))
                                );// user's most recent pending comment (by comment ID just in case next delivery dates get out of relative order by accident)



                            if (ut.userTicketNotes.Count(n => n.EventGeneratedByUser != n.NotifyUser) > 0)// at least 1 note generated by user other than recipient
                            {
                                if
                                (
                                    (minNextDeliveryTime.Value.AddMinutes(consolidationWait) <= now) || // min delivery date is past the maximum allowable delay
                                    (maxPendingDeliveryTime <= now)//max pending is ready to go... commit them all
                                )
                                {
                                    List<TicketEventNotification> consolitations = new List<TicketEventNotification>();
                                    foreach (var note in ut.userTicketNotes)
                                    {
                                        if (note != maxPending)
                                        {
                                            note.Status = "consolidating";
                                            consolitations.Add(note);
                                        }
                                    }
                                    SendTicketEventNotificationEmail(maxPending, consolitations);//consolitations will be null if only 1 message
                                    alteredNotes.Add(maxPending);
                                    alteredNotes.AddRange(consolitations);
                                }
                                else //max pending not yet ready... continue to wait until it is ready before sending
                                {
                                    foreach (var note in ut.userTicketNotes)
                                    {
                                        if (note != maxPending)
                                        {
                                            note.Status = "consolidating";
                                        }
                                        // else : max pending not ready yet - do nothing for max pending (will get sent later)

                                    }
                                    alteredNotes.AddRange(ut.userTicketNotes);
                                    
                                }
                            }
                            else // all waiting are for events generated by recipient
                            {
                                if
                                (
                                    (minNextDeliveryTime.Value.AddMinutes(consolidationWait) <= now) || // min delivery date is past the maximum allowable delay
                                    (maxPendingDeliveryTime <= now)//max pending is ready to go... commit them all
                                )
                                {
                                    //max ready or min past max wait time, but all notes are for events by recipient... kill them all...

                                    foreach (var note in ut.userTicketNotes)
                                    {
                                        note.NextDeliveryAttemptDate = null;
                                        note.Status = "suppressed";
                                        note.LastDeliveryAttemptDate = now;
                                    }
                                    alteredNotes.AddRange(ut.userTicketNotes);
                                }
                                //else max note ready, do nothing
                            }
                        }
                        //else : no notes ready - do nothing
                    }
                }
                finally
                {
                    NotificationRepository.UpdateNotifications(alteredNotes);
                }
            }
        }

        private static object sendingLock = new object();


        private void SendTicketEventNotificationEmail(TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            
            lock (sendingLock)
            {
                PrepareTicketEventNotificationForDelivery(note, consolidations);
                DeliverTicketEventNotificationEmail(note, consolidations);
            }
        }



        private void PrepareTicketEventNotificationForDelivery(TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            double resendDelay = GetEmailResendDelayMinutes();

            int maxRetries = GetEmailMaxDeliveryAttempts();

            note.DeliveryAttempts += 1;
            if (note.DeliveryAttempts < maxRetries)
            {
                note.Status = "pending";
                note.NextDeliveryAttemptDate = DateTime.Now.AddMinutes(resendDelay * note.DeliveryAttempts);
                int backTrackSeconds = -1;
                foreach (var consolidateNote in consolidations.OrderByDescending(c => c.CommentId))
                {
                    //this will reorder the next delivery attempt date for consolidations in descending order keeping the relative order for them all
                    consolidateNote.NextDeliveryAttemptDate = note.NextDeliveryAttemptDate.Value.AddSeconds(backTrackSeconds);
                    consolidateNote.DeliveryAttempts += 1;
                    backTrackSeconds--;
                }
            }
            else
            {
                note.Status = "final-attempt";
                note.NextDeliveryAttemptDate = null;
                foreach (var consolidateNote in consolidations)
                {
                    consolidateNote.NextDeliveryAttemptDate = null;
                    consolidateNote.DeliveryAttempts += 1;
                }
            }
            

            /* Unhandled edge case: 
             *      We've set the next delivery date for the note we are sending to a future date in case something seriously goes wrong during sending the email. 
             *      This is important because if the sending routine exits and we hadn't committed a next delivery date and all that, it would potentially cause
             *      an inifite retry-loop.
             *      
             *      The edge case is that new notes could have been queued after we read the queue from the DB into memory. Those notes would have a delivery 
             *      date much lower than the retry we just scheduled for this note. We could try to predict and work around this potential case, but it is 
             *      such an extreme edge case (something has to queue AND an unhandled exception has to happen in the sending routine both). 
             *      
             *      By not handling this here, the worst that can happen is that the new queued note will simply cause a retry far sooner than we wanted.
             *      A work-around for this case has a significant perforance impact though, and so we are not going to implement that here... besides, if  
             *      there is an unhandled exception in the send routine it will likely be something so severe that future send attemps are all going to fail
             *      for this user's notes indefinitly until the issue is debugged or a major data corruption issue is fixed anyway.
             */
        }


        private void DeliverTicketEventNotificationEmail(TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            bool status = false;

            try
            {
                //TODO: Modify body generation to flag comments for the current event and all consolidations.
                TicketComment comment = note.TicketComment;
                Ticket ticket = comment.Ticket;

               

                int minComment = note.CommentId;
                if (consolidations.Count() > 0)
                {
                    minComment = consolidations.Min(c => c.CommentId);
                }
                string htmlBody = GetHtmlBody(note, note.NotifyUser, minComment);
                string textBody = GetTextBody(note, note.NotifyUser, minComment);

                string displayFrom = GetFromEmailDisplayName();
                string addressFrom = GetFromEmailAddress();
                string blindCopyTo = GetBlindCopyToEmailAddress();

                string tStat = ticket.CurrentStatus;
                if (string.IsNullOrEmpty(ticket.AssignedTo))
                {
                    tStat = (ticket.TicketComments.Count() < 2) ? "New" : "Unassigned";
                }
                string subject = string.Format("Ticket {0} ({1}): {2}", ticket.TicketId.ToString(), tStat, ticket.Title);

                status = EmailHandler.SendEmail(addressFrom, displayFrom, note.NotifyEmail, note.NotifyUserDisplayName, blindCopyTo, subject, htmlBody, textBody);

            }
            catch(Exception ex)
            {
                var exception = new ApplicationException("Failure in Email Delivery", ex);
                //TODO: we need to log/record this here and NOT rethrow it 
            }


            //TODO: There remains a small possibility that a note could be send above, but the below database update might break.
            //      this is a case that could lead to a user being notified multiple times.
            //      I would like to avoid the transactions namespace since it can require MSDTC in some cases.  

            //wrap-up after sending, and update the notification status
            DateTime now = DateTime.Now;
            note.LastDeliveryAttemptDate = now;
            foreach (var consolidateNote in consolidations)
            {
                consolidateNote.LastDeliveryAttemptDate = now;
            }

            if (!status)
            {
                if (note.Status == "final-attempt")
                {
                    note.Status = "failed";
                    foreach (var consolidateNote in consolidations)
                    {
                        consolidateNote.Status = "consolidated-failed";
                    }
                }
                if (note.Status == "pending")
                {
                    note.Status = "queued";
                }

                //check for notes that queued after the send process first started here, and increment their nextDeliveryDate so they don't cause 
                //  resends to happen sooner than the next delivery retry interval warrants. This is only needed when we are rescheduling a note 
                //  for later delivery.
                NotificationRepository.EnsureValidNextDeliveryDateForNewlyQueudNotifications(note.TicketId, note.NotifyUser, note.CommentId, note.NextDeliveryAttemptDate.Value);
            }
            else
            {
                note.Status = "sent";
                note.NextDeliveryAttemptDate = null;
                foreach (var consolidateNote in consolidations)
                {
                    consolidateNote.Status = "consolidated";
                    consolidateNote.NextDeliveryAttemptDate = null;

                }

            }
        }

        private string GetHtmlBody(TicketEventNotification note, string notifyUser, int minComment)
        {
            return GetTicketNotificationHtmlEmailContent(note, minComment);
        }

        private string GetTextBody(TicketEventNotification note, string notifyUser, int minComment)
        {
            return GetTicketNotificationTextEmailContent(note, minComment);
        }



    }
}

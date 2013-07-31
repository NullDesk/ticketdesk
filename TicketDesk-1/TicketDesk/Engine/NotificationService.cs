// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//      Steven Murawski 
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using TicketDesk.Controls;
using TicketDesk.Engine.Linq;
using System.Net.Mail;
using System.Web;

namespace TicketDesk.Engine
{
    public static class NotificationService
    {
        delegate int TicketNotificationDelegate(int ticketid, string url);

        static List<int> CurrentNotifications = new List<int>();

        private static object registerLock = new object();

        /// <summary>
        /// Queues the ticket event notification.
        /// </summary>
        /// <param name="comment">The comment associated with the event.</param>
        public static void QueueTicketEventNotification(TicketComment comment)
        {
            lock (registerLock)
            {
                string enabled = ConfigurationManager.AppSettings["EnableEmailNotifications"];
                if (!string.IsNullOrEmpty(enabled) && Convert.ToBoolean(enabled))
                {

                    TicketDataDataContext ctx = new TicketDataDataContext();
                    var ticket = comment.Ticket;
                    var newNotes = ticket.CreateTicketEventNotificationsForComment(comment.CommentId, comment.CommentedBy);
                    foreach (var note in newNotes)
                    {
                        note.CreatedDate = DateTime.Now;
                        note.DeliveryAttempts = 0;

                        if (note.NotifyEmail == "invalid")//notes with invalid email still added to table, but squash delivery schedule.
                        {
                            note.Status = "invalid";
                            note.NextDeliveryAttemptDate = null;
                            note.LastDeliveryAttemptDate = DateTime.Now;
                        }
                        else
                        {


                            note.Status = "queued";
                            var now = DateTime.Now;
                            note.NextDeliveryAttemptDate = now;

                            if (note.NotifyUserReason != "HelpDesk")// for non-broadcasts to helpdesk schedule on the delay 
                            {
                                string delay = ConfigurationManager.AppSettings["EmailNotificationInitialDelayMinutes"];
                                if (!string.IsNullOrEmpty(delay))
                                {
                                    note.NextDeliveryAttemptDate = now.AddMinutes(Convert.ToDouble(delay));
                                }
                            }
                        }
                    }

                    ctx.TicketEventNotifications.InsertAllOnSubmit(newNotes);
                    ctx.SubmitChanges();


                    //Deliver helpdesk broadcasts NOW! New ticket notifications for help desk only 
                    //  happen for brand-new tickets. These users aren't included on further changes
                    //  to the ticket either so there is no reason to wait for consolidations.
                    //
                    //  Since the creation of notes ensures that tickets with a reason of "HelpDesk" 
                    //    are only for tickets where the recipient is not the owner/assigned user we
                    //    don't have to worry about suppressions or consolidations; we can directly 
                    //    send without worrying about the pre-processing that happens with timer 
                    //    triggered mechanism.
                    foreach (var note in newNotes)
                    {
                        if (note.NotifyUserReason == "HelpDesk" && note.NextDeliveryAttemptDate != null)
                        {
                            SendTicketEventNotificationEmail(ctx, note, new List<TicketEventNotification>());
                        }
                    }
                    
                }
            }
        }

        private static object processLock;
        /// <summary>
        /// Sends the waiting ticket event notifications.
        /// </summary>
        public static void ProcessWaitingTicketEventNotifications()
        {
            processLock = new object();
            lock (processLock)
            {
                var now = DateTime.Now;

                TicketDataDataContext ctx = new TicketDataDataContext();
                var queuedNotes = from n in ctx.TicketEventNotifications
                                  where n.NextDeliveryAttemptDate.HasValue
                                  orderby n.NextDeliveryAttemptDate
                                  group n by new { n.NotifyUser, n.TicketId } into userTicketNotes
                                  select new { userTicketNotes.Key.NotifyUser, userTicketNotes.Key.TicketId, userTicketNotes };

                foreach (var ut in queuedNotes)
                {


                    string maxConsolidationWait = ConfigurationManager.AppSettings["EmailMaxConsolidationWaitMinutes"];
                    double consolidationWait = 10D;//10 minutes is default max wait
                    if (!string.IsNullOrEmpty(maxConsolidationWait))
                    {
                        consolidationWait = Convert.ToDouble(maxConsolidationWait);
                    }

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
                                SendTicketEventNotificationEmail(ctx, maxPending, consolitations);//consolitations will be null if only 1 message
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
                                ctx.SubmitChanges();
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
                                ctx.SubmitChanges();
                            }
                            //else max note ready, do nothing
                        }
                    }
                    //else : no notes ready - do nothing
                }
            }
        }

        private static object sendingLock;
        

        private static void SendTicketEventNotificationEmail(TicketDataDataContext ctx, TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            sendingLock = new object();
            lock (sendingLock)
            {
                PrepareTicketEventNotificationForDelivery(ctx, note, consolidations);
                DeliverTicketEventNotificationEmail(ctx, note, consolidations);
            }
        }



        private static void PrepareTicketEventNotificationForDelivery(TicketDataDataContext ctx, TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            string resendDelaySetting = ConfigurationManager.AppSettings["EmailResendDelayMinutes"];
            double resendDelay = 5D;
            if (!string.IsNullOrEmpty(resendDelaySetting))
            {
                resendDelay = Convert.ToDouble(resendDelay);
            }

            string maxRetriesSetting = ConfigurationManager.AppSettings["EmailMaxDeliveryAttempts"];
            int maxRetries = 5;
            if (!string.IsNullOrEmpty(maxRetriesSetting))
            {
                maxRetries = Convert.ToInt32(maxRetriesSetting);
            }

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

            ctx.SubmitChanges();//submit changes before email attempt... this way if there is an unhandled failure, the ticket will still have incremented the delivery attempt and next retry values (prevents accidental spam)

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


        private static void DeliverTicketEventNotificationEmail(TicketDataDataContext ctx, TicketEventNotification note, List<TicketEventNotification> consolidations)
        {
            bool status = false;

            try
            {
                //TODO: Modify body generation to flag comments for the current event and all consolidations.
                TicketComment comment = note.TicketComment;
                Ticket ticket = comment.Ticket;

                SmtpClient client = new SmtpClient();
                string url = null;
                string rootUrl = ConfigurationManager.AppSettings["WebRootUrlForEmailLinks"];
                if (!string.IsNullOrEmpty(rootUrl))
                {
                    if (!rootUrl.EndsWith("/"))
                    {
                        rootUrl += "/";
                    }
                    url = string.Format("{0}ViewTicket.aspx?id={1}", rootUrl, ticket.TicketId.ToString());
                }

                int minComment = note.CommentId;
                if (consolidations.Count() > 0)
                {
                    minComment = consolidations.Min(c => c.CommentId);
                }
                string body = NotificationUtilities.GetHTMLBody(ticket, url, note.NotifyUser, minComment);


                string displayFrom = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string addressFrom = ConfigurationManager.AppSettings["FromEmailAddress"];
                string blindCopyTo = ConfigurationManager.AppSettings["BlindCopyToEmailAddress"];
                MailAddress bccAddr = null;
                if (!string.IsNullOrEmpty(blindCopyTo))
                {
                    bccAddr = new MailAddress(blindCopyTo);
                }

                MailAddress fromAddr = new MailAddress(addressFrom, displayFrom);
                string tStat = ticket.CurrentStatus;
                if (string.IsNullOrEmpty(ticket.AssignedTo))
                {
                    tStat = (ticket.TicketComments.Count() < 2)? "New" : "Unassigned";
                }
                string subject = string.Format("Ticket {0} ({1}): {2}", ticket.TicketId.ToString(), tStat, ticket.Title);
                MailAddress toAddr = new MailAddress(note.NotifyEmail, note.NotifyUserDisplayName);

                MailMessage msg = new MailMessage(fromAddr, toAddr);
                if (bccAddr != null)
                {
                    msg.Bcc.Add(bccAddr);
                }
                msg.Subject = subject;
                msg.Body = body;  //body;
                msg.IsBodyHtml = true;

                client.Send(msg);

                status = true;
            }
            catch (Exception ex)
            {
                Exception newEx = new ApplicationException("Failure in Email Delivery Timer", ex);
                HttpContext mockContext = (new TicketDesk.Engine.MockHttpContext(false)).Context;
                Elmah.ErrorLog.GetDefault(mockContext).Log(new Elmah.Error(newEx));
            }
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
                //  resends to happen sooner than the next delivery retry interval warrants.
                TicketDataDataContext ctxNew = new TicketDataDataContext();//existing context will have cached data from the past read. Do this on new context
                var newNotes = from tn in ctxNew.TicketEventNotifications
                               where tn.CommentId > note.CommentId && tn.NextDeliveryAttemptDate <= note.NextDeliveryAttemptDate
                               select tn;
                if (newNotes.Count() > 0)
                {
                    int advanceSeconds = 1;
                    foreach (var newNote in newNotes)
                    {
                        newNote.NextDeliveryAttemptDate = note.NextDeliveryAttemptDate.Value.AddSeconds(advanceSeconds);
                        advanceSeconds++;
                    }
                    ctxNew.SubmitChanges();
                }
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
            ctx.SubmitChanges();//submit changes ticket-by-ticket so a failure later doesn't scrap the status update for tickets previously sent

        }
    }
}

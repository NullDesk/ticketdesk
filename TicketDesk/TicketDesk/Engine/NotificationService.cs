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
                    var newNotes = ticket.CreateTicketEventNotificationsForComment(comment.CommentId);
                    foreach (var note in newNotes)
                    {
                        note.CreatedDate = DateTime.Now;
                        note.DeliveryAttempts = 0;
                        note.Status = "queued";

                        DateTime nextDelivery = DateTime.Now;
                        string delay = ConfigurationManager.AppSettings["EmailNotificationInitialDelayMinutes"];
                        if (!string.IsNullOrEmpty(delay))
                        {
                            nextDelivery = nextDelivery.AddMinutes(Convert.ToDouble(delay));
                        }
                       
                        note.NextDeliveryAttemptDate = nextDelivery;
                    }

                    ctx.TicketEventNotifications.InsertAllOnSubmit(newNotes);
                    ctx.SubmitChanges();
                }
            }
        }

        private static object sendingLock = new object();
        /// <summary>
        /// Sends the waiting ticket event notifications.
        /// </summary>
        public static void SendWaitingTicketEventNotifications()
        {
            lock (sendingLock)
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

                var now = DateTime.Now;

                TicketDataDataContext ctx = new TicketDataDataContext();
                var queuedNotes = from n in ctx.TicketEventNotifications
                                  where n.NextDeliveryAttemptDate.HasValue && n.NextDeliveryAttemptDate.Value <= now
                                  select n;

                foreach (var note in queuedNotes)
                {
                    bool status = false;
                    note.LastDeliveryAttemptDate = now;

                    note.DeliveryAttempts += 1;
                    if (note.DeliveryAttempts <= maxRetries)
                    {
                        note.Status = "pending";
                        note.NextDeliveryAttemptDate = now.AddMinutes(resendDelay * note.DeliveryAttempts);
                    }
                    else
                    {
                        note.Status = "final-attempt";
                        note.NextDeliveryAttemptDate = null;
                    }
                    ctx.SubmitChanges();//submit changes before email attempt... this way if there is an unhandled failure, the ticket will still have incremented the delivery attempt and next retry values (prevents accidental spam)

                    status = DeliverTicketEventNotificationEmail(note);

                    if (!status)
                    {
                        if (note.Status == "final-attempt")
                        {
                            note.Status = "failed";
                        }
                        if (note.Status == "pending")
                        {
                            note.Status = "queued";
                        }
                    }
                    else
                    {
                        note.Status = "sent";
                        note.NextDeliveryAttemptDate = null;
                    }
                    ctx.SubmitChanges();//submit changes ticket-by-ticket so a failure later doesn't scrap the status update for tickets previously sent
                }
            }
        }

        /// <summary>
        /// Delivers the ticket event notification email.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>true if email send successful</returns>
        private static bool DeliverTicketEventNotificationEmail(TicketEventNotification note)
        {
            bool status = false;
            try
            {

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
                    url = string.Format("{0}ViewTicket.aspx? id = {1}", rootUrl, ticket.TicketId.ToString());
                }


                string body = NotificationUtilities.GetHTMLBody(ticket, url);


                string displayFrom = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string addressFrom = ConfigurationManager.AppSettings["FromEmailAddress"];
                string blindCopyTo = ConfigurationManager.AppSettings["BlindCopyToEmailAddress"];
                MailAddress bccAddr = null;
                if (!string.IsNullOrEmpty(blindCopyTo))
                {
                    bccAddr = new MailAddress(blindCopyTo);
                }

                MailAddress fromAddr = new MailAddress(addressFrom, displayFrom);

                string subject = string.Format("Ticket {0} changed - {1} {2}", ticket.TicketId.ToString(), SecurityManager.GetUserDisplayName(comment.CommentedBy), comment.CommentEvent);
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
            return status;
        }
    }
}

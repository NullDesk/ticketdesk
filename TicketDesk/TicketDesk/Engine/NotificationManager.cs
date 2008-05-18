// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using TicketDesk.Controls;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Engine
{
    /// <summary>
    /// Provides static methods used to send email notifications to users when a ticket changes
    /// </summary>
    public static class NotificationManager
    {

        /// <summary>
        /// Sends an email notification to the ticket owner and/or assigned user.
        /// </summary>
        /// <remarks>
        /// The notification's body is rendered from a user control and the contents are
        /// output as the message body.
        /// </remarks>
        /// <param name="ticket">The ticket to notifify the user about.</param>
        /// <param name="page">
        /// A page object that can be used to instantiate an instance of a user 
        /// control. Usually the page object that invoked this method.
        /// </param>
        public static void SendTicketChangedNotification(Ticket ticket, Page page)
        {
            bool enableEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmailNotifications"]);
            if(enableEmail)
            {
                SmtpClient client = new SmtpClient();
                    
                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);
                HtmlTextWriter htmWriter = new HtmlTextWriter(stringWriter);


                EmailTicketView view = (EmailTicketView)page.LoadControl("~/Controls/EmailTicketView.ascx");
                view.TicketToDisplay = ticket;

                view.Populate(page.Request.Url.AbsoluteUri);
                view.RenderControl(htmWriter);

                TicketComment comment = ticket.TicketComments.Single(tc => tc.CommentedDate == (ticket.TicketComments.Max(tcm => tcm.CommentedDate)));

               
                string displayFrom = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string addressFrom = ConfigurationManager.AppSettings["FromEmailAddress"];
                MailAddress fromAddr = new MailAddress(addressFrom, displayFrom);

                string subject = string.Format("Ticket {0} changed - {1} {2}", ticket.TicketId.ToString(), comment.CommentedBy, comment.CommentEvent);
                string body = string.Format("{0}{1}{2}", "<html><head></head><body>", stringBuilder.ToString(), "</body></html>");
                foreach(MailAddress toAddr in ticket.GetNotificationEmailAddressesForUsers())
                {
                    MailMessage msg = new MailMessage(fromAddr, toAddr);
                    msg.Subject = subject;
                    msg.Body = body;//body;
                    msg.IsBodyHtml = true;
                    //msg.BodyEncoding = Encoding.UTF8;
                    //msg.SubjectEncoding = Encoding.UTF8;
                    try
                    {
                        client.Send(msg);
                    }
                    catch
                    {
                        //do nothing, continue
                    }
                }
            }
        }

        
    }
}

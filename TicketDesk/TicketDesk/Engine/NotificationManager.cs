using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;
using System.Text;
using System.IO;
using TicketDesk.Controls;
using System.Net.Mail;
using System.Collections.Generic;

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
                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);
                HtmlTextWriter htmWriter = new HtmlTextWriter(stringWriter);


                EmailTicketView view = (EmailTicketView)page.LoadControl("~/Controls/EmailTicketView.ascx");
                view.TicketToDisplay = ticket;

                view.Populate(page.Request.Url.AbsoluteUri);
                view.RenderControl(htmWriter);

                TicketComment comment = ticket.TicketComments.Single(tc => tc.CommentedDate == (ticket.TicketComments.Max(tcm => tcm.CommentedDate)));

                List<MailAddress> addressesToSendTo = new List<MailAddress>();
                if(!string.IsNullOrEmpty(ticket.Owner) && ticket.Owner != ticket.LastUpdateBy)
                {
                    AddEmailForUser(ticket.Owner, addressesToSendTo);
                }
                if(!string.IsNullOrEmpty(ticket.AssignedTo) && ticket.AssignedTo != ticket.LastUpdateBy)
                {
                    AddEmailForUser(ticket.AssignedTo, addressesToSendTo);
                }
                string displayFrom = ConfigurationManager.AppSettings["FromEmailDisplayName"];
                string addressFrom = ConfigurationManager.AppSettings["FromEmailAddress"];
                MailAddress fromAddr = new MailAddress(addressFrom, displayFrom);

                string subject = string.Format("Ticket {0} changed - {1} {2}", ticket.TicketId.ToString(), comment.CommentedBy, comment.CommentEvent);
                string body = string.Format("{0}{1}{2}", "<html><head></head><body>", stringBuilder.ToString(), "</body></html>");
                foreach(MailAddress toAddr in addressesToSendTo)
                {
                    MailMessage msg = new MailMessage(fromAddr, toAddr);
                    msg.Subject = subject;
                    msg.Body = body;//body;
                    msg.IsBodyHtml = true;
                    //msg.BodyEncoding = Encoding.UTF8;
                    //msg.SubjectEncoding = Encoding.UTF8;
                    SmtpClient client = new SmtpClient();
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

        /// <summary>
        /// Adds the email address for a user to a list of addresses.
        /// </summary>
        /// <param name="user">The user whose email should be added.</param>
        /// <param name="addressesToSendTo">The collection of addresses to 
        /// add the user's email address to.</param>
        private static void AddEmailForUser(string user, List<MailAddress> addressesToSendTo)
        {
            string email = SecurityManager.GetUserEmailAddress(user);
            if(!string.IsNullOrEmpty(email))
            {
                MailAddress addy = new MailAddress(email, SecurityManager.GetUserDisplayName(user));
                addressesToSendTo.Add(addy);
            }
        }
    }
}

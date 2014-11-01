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
using System.Net.Mail;
using System.ComponentModel.Composition;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(IEmailHandlerService))]
    [ExportMetadata("EmailServiceName", "DefaultEmailHandler")]
    public class DefaultSmtpEmailHandlerService : IEmailHandlerService
    {
     
        /// <summary>
        /// Sends the email via the .NET SMTP client as configured in web.config.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="fromDisplay">From display.</param>
        /// <param name="toAddress">To address.</param>
        /// <param name="toDisplay">To display.</param>
        /// <param name="bccAddress">The BCC address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlBody">The HTML body.</param>
        /// <param name="textBody">The text body.</param>
        /// <returns></returns>
        public bool SendEmail(string fromAddress, string fromDisplay, string toAddress, string toDisplay, string bccAddress, string subject, string htmlBody, string textBody)
        {
            SmtpClient client = new SmtpClient();
            MailAddress bccAddr = null;
            if (!string.IsNullOrEmpty(bccAddress))
            {
                bccAddr = new MailAddress(bccAddress);
            }

            MailAddress fromAddr = new MailAddress(fromAddress, fromDisplay);

            MailAddress toAddr = new MailAddress(toAddress, toDisplay);//notification.NotifyEmail, notification.NotifyUserDisplayName);

            MailMessage msg = new MailMessage(fromAddr, toAddr);
            if (bccAddr != null)
            {
                msg.Bcc.Add(bccAddr);
            }
            msg.Subject = subject;

            msg.Body = textBody;  
            
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, new System.Net.Mime.ContentType("text/html"));
            msg.AlternateViews.Add(htmlView);
            client.Send(msg);
            return true;
        }
    }
}

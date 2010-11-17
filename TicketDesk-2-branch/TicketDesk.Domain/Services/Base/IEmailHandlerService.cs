using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Services
{
    public interface IEmailHandlerService
    {
        /// <summary>
        /// Sends an email.
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
        bool SendEmail(string fromAddress, string fromDisplay, string toAddress, string toDisplay, string bccAddress, string subject, string htmlBody, string textBody);
       
    }
}

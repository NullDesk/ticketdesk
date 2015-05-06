// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using ActionMailerNext.Standalone;

namespace TicketDesk.Mailer
{
    public class TicketMailController : RazorMailerBase
    {
        public override string ViewPath
        {
            get { return "Templates"; }
        }
        public RazorEmailResult TicketEmail(int ticketId, int[] includedEvents, string subscriberEmail, string subscriberName)
        {
            //// Setting up needed properties
            //MailAttributes.From = new MailAddress("d.beckham@someclub.com", "Mr. Beckham");
            //MailAttributes.To.Add(new MailAddress(model.EmailAddress));
            //MailAttributes.Subject = "Check my new sunglasses";
            //MailAttributes.Priority = MailPriority.High;

            ////Calling the view which form the email body
            //return Email("newSunglassesView", model);
            return null;
        }
    }
}   

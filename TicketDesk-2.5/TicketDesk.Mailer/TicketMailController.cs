using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

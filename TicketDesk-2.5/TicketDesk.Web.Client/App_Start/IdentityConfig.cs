using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TicketDesk.Web.Client
{
    //TODO: This file is from the sample template, leaving here for now until I figure out what to do with two factor and where to put it



    //TODO: What to do with this?
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
    //TODO: What to do with this?
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }




}

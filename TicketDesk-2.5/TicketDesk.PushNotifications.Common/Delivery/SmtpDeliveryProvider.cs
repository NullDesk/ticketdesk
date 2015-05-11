using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common
{
    [Description("SMTP Provider")]
    public sealed class SmtpDeliveryProvider : PushNotificationDeliveryProviderBase
    {
        public SmtpDeliveryProvider(JToken configuration)
        {
            Configuration = configuration == null ? 
                new SmtpDeliveryProviderConfiguration() :
                configuration.ToObject<SmtpDeliveryProviderConfiguration>();
        }

        public override IDeliveryProviderConfiguration Configuration { get; set; }

        public override string DestinationType
        {
            get { return "Email"; }
        }

        public override Task<object> GenerateMessageAsync(PushNotificationItem notificationItem)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message)
        {
            var client = new SmtpClient();
            MailMessage msg = (MailMessage)message;
            client.Send(msg);
            return Task.FromResult(true);
        }
    }


}

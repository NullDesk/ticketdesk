using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace TicketDesk.PushNotifications.Common
{
    [Description("SMTP Provider")]
    public sealed class SmtpDeliveryProvider : PushNotificationDeliveryProviderBase
    {
        public SmtpDeliveryProvider(object configuration)
        {
            Configuration = configuration == null ? 
                new SmtpDeliveryProviderConfiguration() :
                (SmtpDeliveryProviderConfiguration)configuration;
        }

        public override IDeliveryProviderConfiguration Configuration { get; set; }

        public override string DestinationType
        {
            get { return "Email"; }
        }

        public override Task<object> GenerateMessageAsync(Model.PushNotificationItem notificationItem)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> SendNotificationAsync(object message)
        {
            throw new NotImplementedException();
        }


        public class SmtpDeliveryProviderConfiguration : IDeliveryProviderConfiguration
        {
            public SmtpDeliveryProviderConfiguration()
            {
                SmtpServer = "localhost";
                SmtpPort = 25;
                EnableSsl = false;

            }

            public string SmtpServer { get; set; }
            public int? SmtpPort { get; set; }
            public bool? EnableSsl { get; set; }
            public string SmtpUserName { get; set; }
            public string SmtpPassword { get; set; }
            public string SmtpFromAddress { get; set; }



        }
    }


}

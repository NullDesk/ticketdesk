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

            [Display(Name="SMTP Server Name")]
            public string SmtpServer { get; set; }

            [Display(Name = "SMTP Port")]
            public int? SmtpPort { get; set; }

            [Display(Name = "Enable SSL")]
            public bool? EnableSsl { get; set; }

            [Display(Name = "SMTP User Name")]
            [Description("Leave empty if authentication is not required")]
            public string SmtpUserName { get; set; }

            [Display(Name = "SMTP Password")]
            [Description("Leave empty if authentication is not required")]
            public string SmtpPassword { get; set; }

            [Display(Name = "SMTP From Address")]
            [Description("The email address to use when sending email from this povider")]
            public string SmtpFromAddress { get; set; }

            [Display(Name = "SMTP From Display Name")]
            [Description("The friendly name to use in the from address")]
            public string SmtpFromDisplayName { get; set; }




        }
    }


}

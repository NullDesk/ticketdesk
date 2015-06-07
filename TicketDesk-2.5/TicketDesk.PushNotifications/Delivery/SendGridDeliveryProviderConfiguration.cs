using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using S22.Mail;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Delivery
{
    public class SendGridDeliveryProviderConfiguration : IDeliveryProviderConfiguration
    {
        [Display(Name = "ApiKey")]
        [Description("The ApiKey used to authenticate with SendGrid (this provider does not support username/password credentials).")]
        [Required]
        [DataType(DataType.Password)]
        public string ApiKey { get; set; }

        [Display(Name = "From Address")]
        [Description("The email address to use when sending email from this povider")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string FromAddress { get; set; }

        [Display(Name = "From Display Name")]
        [Description("The friendly name to use in the from address")]
        [Required]
        public string FromDisplayName { get; set; }

        [Display(Name = "Click Tracking", Prompt = "Enabled")]
        [Description("Enables SendGrid's click tracking feature")]
        public bool? EnableClickTracking { get; set; }

        [Display(Name = "Open Tracking", Prompt = "Enabled")]
        [Description("Enables SendGrid's open tracking feature")]
        public bool? EnableOpenTracking { get; set; }

        [Display(Name = "Gravatar", Prompt = "Enabled")]
        [Description("Enables SendGrid's gravatar feature (you should make sure the from account configured here has a gravatar account)")]
        public bool? EnableGravatar { get; set; }

        [Display(Name = "Send to Sink", Prompt = "Enabled")]
        [Description("Sends all emails to SendGrid's sink address (useful when testing email delivery to prevent sending mail to real recipients)")]
        public bool? SendToSink { get; set; }
    }
}

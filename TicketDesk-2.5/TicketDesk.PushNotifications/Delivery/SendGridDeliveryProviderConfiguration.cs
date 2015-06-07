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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

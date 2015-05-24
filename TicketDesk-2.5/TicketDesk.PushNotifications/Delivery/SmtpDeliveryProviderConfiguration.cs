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

namespace TicketDesk.PushNotifications
{
    public class SmtpDeliveryProviderConfiguration : IDeliveryProviderConfiguration
    {
        public SmtpDeliveryProviderConfiguration()
        {
            SmtpServer = "localhost";
            SmtpPort = 25;
            EnableSsl = false;
            SmtpFromDisplayName = "TicketDesk";
        }

        [Display(Name = "SMTP Server Name")]
        [Required]
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
        [Required]
        [EmailAddress]
        public string SmtpFromAddress { get; set; }

        [Display(Name = "SMTP From Display Name")]
        [Description("The friendly name to use in the from address")]
        [Required]
        public string SmtpFromDisplayName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

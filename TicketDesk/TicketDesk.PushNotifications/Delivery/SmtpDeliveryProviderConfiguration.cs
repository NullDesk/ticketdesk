// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.ComponentModel.DataAnnotations;
using TicketDesk.Localization;
using TicketDesk.Localization.PushNotifications;

namespace TicketDesk.PushNotifications.Delivery
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

        [Display(Name = "SMTPServerName", ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        public string SmtpServer { get; set; }

        [Display(Name = "SMTPPort", ResourceType = typeof(Strings))]
        public int? SmtpPort { get; set; }

        [Display(Name = "EnableSSL", ResourceType = typeof(Strings))]
        public bool? EnableSsl { get; set; }

        [Display(Name = "SMTPUserName", ResourceType = typeof(Strings))]
        [LocalizedDescription("SMTPUserName_Description", NameResourceType = typeof(Strings))]
        public string SmtpUserName { get; set; }

        [Display(Name = "SMTPPassword", ResourceType = typeof(Strings))]
        [LocalizedDescription("SMTPPassword_Description", NameResourceType = typeof(Strings))]
        [DataType(DataType.Password)]
        public string SmtpPassword { get; set; }

        [Display(Name = "SMTPFromAddress", ResourceType = typeof(Strings))]
        [LocalizedDescription("SMTPFromAddress_Description", NameResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Validation))]
        public string SmtpFromAddress { get; set; }

        [Display(Name = "SMTPFromDisplayName", ResourceType = typeof(Strings))]
        [LocalizedDescription("SMTPFromDisplayName_Description", NameResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        public string SmtpFromDisplayName { get; set; }

    }
}

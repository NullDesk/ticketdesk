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
    public class SendGridDeliveryProviderConfiguration : IDeliveryProviderConfiguration
    {
        [Display(Name = "ApiKey", ResourceType = typeof(Strings))]
        [LocalizedDescription("ApiKey_Description", NameResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [DataType(DataType.Password)]
        public string ApiKey { get; set; }

        [Display(Name = "FromAddress", ResourceType = typeof(Strings))]
        [LocalizedDescription("FromAddress_Description", NameResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Validation))]
        public string FromAddress { get; set; }

        [Display(Name = "FromDisplayName", ResourceType = typeof(Strings))]
        [LocalizedDescription("FromDisplayName_Description", NameResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        public string FromDisplayName { get; set; }

        [Display(Name = "ClickTracking", Prompt = "ClickTracking_Prompt", ResourceType = typeof(Strings))]
        [LocalizedDescription("ClickTracking_Description", NameResourceType = typeof(Strings))]
        public bool? EnableClickTracking { get; set; }

        [Display(Name = "OpenTracking", Prompt = "OpenTracking_Prompt", ResourceType = typeof(Strings))]
        [LocalizedDescription("OpenTracking_Description", NameResourceType = typeof(Strings))]
        public bool? EnableOpenTracking { get; set; }

        [Display(Name = "Gravatar", Prompt = "Gravatar_Prompt", ResourceType = typeof(Strings))]
        [LocalizedDescription("Gravatar_Description", NameResourceType = typeof(Strings))]
        public bool? EnableGravatar { get; set; }

        [Display(Name = "SendToSink", Prompt = "SendToSink_Prompt", ResourceType = typeof(Strings))]
        [LocalizedDescription("SendToSink_Description", NameResourceType = typeof(Strings))]
        public bool? SendToSink { get; set; }
    }
}

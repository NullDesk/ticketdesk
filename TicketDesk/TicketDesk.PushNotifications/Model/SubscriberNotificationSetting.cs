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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketDesk.Localization;
using TicketDesk.Localization.PushNotifications;

namespace TicketDesk.PushNotifications.Model
{
    [Table("SubscriberPushNotificationSettings", Schema = "notifications")]
    public class SubscriberNotificationSetting
    {
        public SubscriberNotificationSetting()
        {
            IsEnabled = true;
            PushNotificationDestinations = new List<PushNotificationDestination>();
        }

        [Key]
        [StringLength(256, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        public string SubscriberId { get; set; }

        [Display(Name = "NotificationsEnabled", Prompt = "NotificationsEnabled_Prompt", ResourceType = typeof(Strings))]
        public bool IsEnabled { get; set; }


        public virtual ICollection<PushNotificationDestination> PushNotificationDestinations { get; set; }

    }
}

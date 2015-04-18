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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketDesk.PushNotifications.Common.Model
{
    [Table("SubscriberPushNotificationSettings", Schema = "notifications")]
    public sealed class SubscriberPushNotificationSetting
    {
        public SubscriberPushNotificationSetting()
        {
            IsEnabled = true;
            PushNotificationDestinations = new PushNotificationDestinationCollection();
        }

        [Key]
        [StringLength(256)]
        public string SubscriberId { get; set; }

        [Display(Name = "Notifications Enabled?")]
        public bool IsEnabled { get; set; }


        public PushNotificationDestinationCollection PushNotificationDestinations { get; set; }

    }
}

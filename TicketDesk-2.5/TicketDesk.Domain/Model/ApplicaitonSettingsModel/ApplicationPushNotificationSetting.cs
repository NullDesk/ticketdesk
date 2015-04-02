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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketDesk.Domain.Annotations;

namespace TicketDesk.Domain.Model
{
    public class ApplicationPushNotificationSetting
    {

        public ApplicationPushNotificationSetting()
        {
            IsEnabled = true;
            DeliveryIntervalMinutes = 2;
            AntiNoiseSettings = new AntiNoiseSetting();
            RetryAttempts = 5;
            RetryIntervalMinutes = 5;
        }

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            [UsedImplicitly]
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                var jsettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                var jData = JsonConvert.DeserializeObject<ApplicationPushNotificationSetting>(value, jsettings);
                IsEnabled = jData.IsEnabled;
                DeliveryIntervalMinutes = jData.DeliveryIntervalMinutes;
                RetryAttempts = jData.RetryAttempts;
                RetryIntervalMinutes = jData.RetryIntervalMinutes;

            }
        }

        [NotMapped]
        [Display(Name = "Notifications Enabled?")]
        public bool IsEnabled { get; set; }

        [NotMapped]
        [Display(Name = "Delivery attempt interval (minutes)")]
        public int DeliveryIntervalMinutes { get; set; }

        [NotMapped]
        [Display(Name = "Number of rety attempts")]
        public int RetryAttempts { get; set; }

        [NotMapped]
        [Display(Name = "Base retry interval (minutes)")]
        public int RetryIntervalMinutes { get; set; }

        [NotMapped]
        public AntiNoiseSetting AntiNoiseSettings { get; set; }

        public class AntiNoiseSetting
        {
            public AntiNoiseSetting()
            {
                IsConsoldationEnabled = true;
                InitialConsolidationDelayMinutes = 6;
                MaxConsolidationDelayMinutes = 16;
                ExcludeSubscriberEvents = true;
            }

            [NotMapped]
            [Display(Name = "Consolidate multple notifications for subscriber into one message?")]
            public bool IsConsoldationEnabled { get; set; }

            [NotMapped]
            [Display(Name = "Initial consolidation delay interval (minutes)")]
            public int InitialConsolidationDelayMinutes { get; set; }

            [NotMapped]
            [Display(Name = "Maximum consolidation delay interval (minutes)")]
            public int MaxConsolidationDelayMinutes { get; set; }

            [NotMapped]
            [Display(Name = "Suppress notificaitons for events created by subscriber?")]
            public bool ExcludeSubscriberEvents { get; set; }
        }
    }
}

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

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicketDesk.PushNotifications.Delivery;

namespace TicketDesk.PushNotifications.Model
{
    [Table("ApplicationPushNotificationSettings", Schema = "notifications")]
    public class ApplicationPushNotificationSetting
    {

        public ApplicationPushNotificationSetting()
        {
            ApplicationName = "TicketDesk";
            IsEnabled = true;
            DeliveryIntervalMinutes = 2;
            AntiNoiseSettings = new AntiNoiseSetting();
            RetryAttempts = 5;
            RetryIntervalMinutes = 2;
            DeliveryProviderSettings = new List<PushNotificationDeliveryProviderSetting> { };
        }

        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(AutoGenerateField = false)]
        public string ApplicationName { get; set; }

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                var jsettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                var jData = JsonConvert.DeserializeObject<ApplicationPushNotificationSetting>(value, jsettings);
                IsEnabled = jData.IsEnabled;
                DeliveryIntervalMinutes = jData.DeliveryIntervalMinutes;
                RetryAttempts = jData.RetryAttempts;
                RetryIntervalMinutes = jData.RetryIntervalMinutes;
                DeliveryProviderSettings = jData.DeliveryProviderSettings;
                AntiNoiseSettings = jData.AntiNoiseSettings;
            }
        }

        [NotMapped]
        [Display(Name="Delivery Providers")]
        public virtual ICollection<PushNotificationDeliveryProviderSetting> DeliveryProviderSettings { get; set; }
            
        [NotMapped]
        [Display(Name = "Notifications Enabled", Prompt = "Enabled")]
        public bool IsEnabled { get; set; }

        [NotMapped]
        [Display(Name = "Delivery Attempt Interval (minutes)")]
        [Description("How often to check for push notifications that are ready to be sent.")]
        public int DeliveryIntervalMinutes { get; set; }

        [NotMapped]
        [Display(Name = "Number of Retry Attempts")]
        [Description("When things go wrong, this determines the number of attempts to make before marking a message as failed.")]
        public int RetryAttempts { get; set; }

        [NotMapped]
        [Display(Name = "Initial Retry Interval (minutes)")]
        [Description(@"Number of minutes to wait before the first retry attempt. Subsequent retry attempts occur at interval raised to the number of retry attempts. Example: interval = 2 and retry = 3, next attempt will be in 8 minutes")]
        public int RetryIntervalMinutes { get; set; }

        [NotMapped]
        public AntiNoiseSetting AntiNoiseSettings { get; set; }


        public class PushNotificationDeliveryProviderSetting
        {
            public PushNotificationDeliveryProviderSetting()
            {
                IsEnabled = false;
            }

            [NotMapped]
            public string ProviderAssemblyQualifiedName { get; set; }

            [NotMapped]
            [Display(Name = "Provider", Prompt = "Enabled")]
            public bool IsEnabled { get; set; }

            [NotMapped]
            public JObject ProviderConfigurationData { get; set; }

            public static PushNotificationDeliveryProviderSetting FromProvider(IPushNotificationDeliveryProvider provider)
            {
                return new PushNotificationDeliveryProviderSetting()
                {
                    IsEnabled = false,
                    ProviderAssemblyQualifiedName = provider.GetType().AssemblyQualifiedName,
                    ProviderConfigurationData = JObject.FromObject(provider.Configuration)
                };
            }
        }


        public class AntiNoiseSetting
        {
            public AntiNoiseSetting()
            {
                IsConsolidationEnabled = true;
                InitialConsolidationDelayMinutes = 6;
                MaxConsolidationDelayMinutes = 16;
                ExcludeSubscriberEvents = true;
            }

            [NotMapped]
            [Display(Name = "Consolidate notifications", Prompt = "Enabled")]
            [Description("Consolidation allows push notifications for an event to be delayed for a short time. If additional events occur for the same subscriber and ticket, they will be grouped into a single message instead of being sent separately. This reduces spam in cases where many chages are made to a ticket in rapid succession.")]
            public bool IsConsolidationEnabled { get; set; }

            [NotMapped]
            [Display(Name = "Initial consolidation delay (minutes)")]
            [Description("The initial number of minutes to wait. If new events occur for the same subscriber and ticket, the system will consolidate the messages into a single notification. Subsequent events will increase the delay until either no further events occur, or the maximum delay period has expired.")]
            public int InitialConsolidationDelayMinutes { get; set; }

            [NotMapped]
            [Display(Name = "Maximum consolidation delay (minutes)")]
            [Description("The maximum amount of time to delay sending a notification.")]
            public int MaxConsolidationDelayMinutes { get; set; }

            [NotMapped]
            [Display(Name = "Exclude subscriber's own events", Prompt = "Exclude")]
            [Description("HIGHLY RECOMMENDED! This prevents push notifications from being sent to the same user whose action triggered the notification.")]
            public bool ExcludeSubscriberEvents { get; set; }
        }
    }
}

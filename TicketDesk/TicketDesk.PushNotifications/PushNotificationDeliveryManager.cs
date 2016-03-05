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

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TicketDesk.PushNotifications.Delivery;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications
{
    public static class PushNotificationDeliveryManager
    {
        private static ICollection<IPushNotificationDeliveryProvider> _deliveryProviders;

        public static ICollection<IPushNotificationDeliveryProvider> DeliveryProviders
        {
            get
            {
                if (_deliveryProviders == null)
                {
                    _deliveryProviders = new List<IPushNotificationDeliveryProvider>();
                    using (var context = new TdPushNotificationContext())
                    {
                        var providerConfigs = context.TicketDeskPushNotificationSettings.DeliveryProviderSettings;
                        foreach (var prov in providerConfigs)
                        {
                            if (prov.IsEnabled)
                            {
                                _deliveryProviders.Add(CreateDeliveryProviderInstance(prov));
                            }
                        }
                    }
                }
                return _deliveryProviders;
            }
            set { _deliveryProviders = value; }
        }

        public static IPushNotificationDeliveryProvider CreateDeliveryProviderInstance(ApplicationPushNotificationSetting.PushNotificationDeliveryProviderSetting settings)
        {
            var provType = Type.GetType(settings.ProviderAssemblyQualifiedName);
            if (provType != null)
            {
                var ci = provType.GetConstructor(new[] {typeof (JObject)});
                if (ci != null)
                {
                   return (IPushNotificationDeliveryProvider) ci.Invoke(new object[] {settings.ProviderConfigurationData});
                }
            }
            return null;
        }

        public static IPushNotificationDeliveryProvider CreateDefaultDeliveryProviderInstance(Type providerType)
        {
            var ci = providerType.GetConstructor(new[] { typeof(JObject) });
            if (ci == null)
            {
                throw new InstanceNotFoundException("Cannot locate a constructor for " + typeof(JObject).Name);
            }
            return (IPushNotificationDeliveryProvider)ci.Invoke(new object[] { null });
        }


        public static async Task SendNotificationAsync
        (
            int contentSourceId,
            string contentSourceType,
            string subscriberId,
            int destinationId
        )
        {
            using (var context = new TdPushNotificationContext())
            {
                var readyNote = await context.PushNotificationItems
                    .FirstOrDefaultAsync(n =>
                        (
                            n.ContentSourceId == contentSourceId &&
                            n.ContentSourceType == contentSourceType &&
                            n.SubscriberId == subscriberId &&
                            n.DestinationId == destinationId
                        ) &&
                        (
                            n.DeliveryStatus == PushNotificationItemStatus.Scheduled ||
                            n.DeliveryStatus == PushNotificationItemStatus.Retrying)
                        );
                if (readyNote == null) { return; }

                await SendNotificationMessageAsync(context, readyNote, CancellationToken.None);
                await context.SaveChangesAsync();
            }
        }

        public static async Task<int> SendNextReadyNotificationAsync(CancellationToken ct)
        {
            using (var context = new TdPushNotificationContext())
            {
                //get the next notification that is ready to send
                var readyNote =
                    await context.PushNotificationItems.Include(n => n.Destination).OrderBy(n => n.ScheduledSendDate).FirstOrDefaultAsync(
                        n =>
                            (n.DeliveryStatus == PushNotificationItemStatus.Scheduled || n.DeliveryStatus == PushNotificationItemStatus.Retrying) &&
                            n.ScheduledSendDate <= DateTimeOffset.Now, ct);

                if (readyNote == null)
                {
                    return 0;
                }
                await SendNotificationMessageAsync(context, readyNote, ct);
                return await context.SaveChangesAsync(ct);
                
            }
        }

        private static async Task SendNotificationMessageAsync(TdPushNotificationContext context, PushNotificationItem readyNote, CancellationToken ct)
        {
            var retryMax = context.TicketDeskPushNotificationSettings.RetryAttempts;
            var retryIntv = context.TicketDeskPushNotificationSettings.RetryIntervalMinutes;

            //find a provider for the notification destination type
            var provider = DeliveryProviders.FirstOrDefault(p => p.DestinationType == readyNote.Destination.DestinationType);
            if (provider == null)
            {
                //no provider
                readyNote.DeliveryStatus = PushNotificationItemStatus.NotAvailable;
                readyNote.ScheduledSendDate = null;
            }
            else
            {
                await provider.SendReadyMessageAsync(readyNote, retryMax, retryIntv, ct);
            }
        }
    }
}

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

using System.Threading;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Delivery
{
    public abstract class PushNotificationDeliveryProviderBase : IPushNotificationDeliveryProvider
    {
    
        public abstract string DestinationType { get; }

        public abstract Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct);

        public abstract Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message, CancellationToken ct);

        public abstract IDeliveryProviderConfiguration Configuration { get; set; }

        public async Task SendReadyMessageAsync(PushNotificationItem notificationItem, int retryMax, int retryIntv, CancellationToken ct)
        {

            //do the meat
            var message = await GenerateMessageAsync(notificationItem, ct);
            var result = await SendNotificationAsync(notificationItem, message, ct);

            //if we're in a retry case, increment retry count
            if (notificationItem.DeliveryStatus == PushNotificationItemStatus.Retrying)
            {
                notificationItem.RetryCount++;
            }
            if (result)
            {
                //if sent, mark sent and remove schedule
                notificationItem.DeliveryStatus = PushNotificationItemStatus.Sent;
                notificationItem.ScheduledSendDate = null;
            }
            else
            {
                if (notificationItem.RetryCount <= retryMax)
                {
                    //mark for retry, update schedule
                    notificationItem.DeliveryStatus = PushNotificationItemStatus.Retrying;
                    if (notificationItem.ScheduledSendDate != null)
                    {
                        notificationItem.ScheduledSendDate =
                            notificationItem.ScheduledSendDate.Value.AddMinutes(retryIntv ^ notificationItem.RetryCount);
                    }
                }
                else
                {
                    //too many retry attempts, mark fail and clear schedule
                    notificationItem.DeliveryStatus = PushNotificationItemStatus.Failed;
                    notificationItem.ScheduledSendDate = null;
                }
            }
        }
    }
}

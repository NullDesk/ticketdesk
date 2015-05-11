using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common
{
    public abstract class PushNotificationDeliveryProviderBase : IPushNotificationDeliveryProvider
    {
        public abstract string DestinationType { get; }

        public abstract Task<object> GenerateMessageAsync(PushNotificationItem notificationItem);

        public abstract Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message);

        public abstract IDeliveryProviderConfiguration Configuration { get; set; }

        public async Task SendReadyMessageAsync(PushNotificationItem notificationItem, int retryMax, int retryIntv)
        {

            //do the meat
            var message = await GenerateMessageAsync(notificationItem);
            var result = await SendNotificationAsync(notificationItem, message);

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

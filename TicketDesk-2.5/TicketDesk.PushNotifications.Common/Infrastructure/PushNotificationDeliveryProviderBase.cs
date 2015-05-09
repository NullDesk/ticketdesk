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

        public abstract Task<bool> SendNotificationAsync(object message);

        public abstract IDeliveryProviderConfiguration Configuration { get; set; }

        public async Task SendReadyMessageAsync(PushNotificationItem readyNote, int retryMax, int retryIntv)
        {

            //do the meat
            var message = await GenerateMessageAsync(readyNote);
            var result = await SendNotificationAsync(message);

            //if we're in a retry case, increment retry count
            if (readyNote.DeliveryStatus == PushNotificationItemStatus.Retrying)
            {
                readyNote.RetryCount++;
            }
            if (result)
            {
                //if sent, mark sent and remove schedule
                readyNote.DeliveryStatus = PushNotificationItemStatus.Sent;
                readyNote.ScheduledSendDate = null;
            }
            else
            {
                if (readyNote.RetryCount <= retryMax)
                {
                    //mark for retry, update schedule
                    readyNote.DeliveryStatus = PushNotificationItemStatus.Retrying;
                    if (readyNote.ScheduledSendDate != null)
                    {
                        readyNote.ScheduledSendDate =
                            readyNote.ScheduledSendDate.Value.AddMinutes(retryIntv ^ readyNote.RetryCount);
                    }
                }
                else
                {
                    //too many retry attempts, mark fail and clear schedule
                    readyNote.DeliveryStatus = PushNotificationItemStatus.Failed;
                    readyNote.ScheduledSendDate = null;
                }

            }
        }

    }
}

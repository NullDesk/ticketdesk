using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Delivery
{
    public abstract class EmailDeliveryProviderBase: PushNotificationDeliveryProviderBase
    {
        public override IDeliveryProviderConfiguration Configuration { get; set; }

        public override string DestinationType
        {
            get { return "email"; }
        }

        public override Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct)
        {
            var memorydata = Convert.FromBase64String(notificationItem.MessageContent);
            using (var rs = new MemoryStream(memorydata))
            {
                var sf = new BinaryFormatter();
                return Task.FromResult(sf.Deserialize(rs));
            }
        }
    }
}

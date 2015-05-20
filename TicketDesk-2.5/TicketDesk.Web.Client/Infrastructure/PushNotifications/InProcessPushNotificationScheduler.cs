using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.PushNotifications;

namespace TicketDesk.Web.Client.Infrastructure
{
    public class InProcessPushNotificationScheduler
    {
        private static readonly Timer Timer = new Timer(OnTimerElapsed);

        public static void Start(int interval)
        {
            Timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval * 60 * 1000));
        }

        private static void OnTimerElapsed(object sender)
        {
            HostingEnvironment.QueueBackgroundWorkItem(
                async ct =>
                {
                    var i = 1;//await PushNotificationDeliveryManager.SendNextReadyNotification(ct);
                        while (i > 0)
                    {
                        i = await PushNotificationDeliveryManager.SendNextReadyNotification(ct);
                    }
                });

        }
    }
}
using System;
using System.Threading;
using Microsoft.Azure.WebJobs;

namespace TicketDesk.PushNotifications.Job
{
    public class Functions
    {
        private static readonly Timer Timer = new Timer(OnTimerElapsed);

        private static void OnTimerElapsed(object sender)
        {
            Console.Out.WriteLine("delivery timer elapsed");
            var i = 1;
            while (i > 0)
            {
                var task = PushNotificationDeliveryManager.SendNextReadyNotificationAsync(CancellationToken.None);
                i = task.Result;
                if (i > 0) { Console.Out.WriteLine("one message delivered"); }
            }
            Console.Out.WriteLine("delivery complete");
        }

        [NoAutomaticTrigger]
        public static void StartNotificationScheduler(int interval)
        {
            Console.Out.WriteLine("starting delivery timer");
            Timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(interval * 60 * 1000));
        }
    }
}

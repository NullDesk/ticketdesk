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
using System.Threading;
using System.Web.Hosting;
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
                    var i = 1;
                        while (i > 0)
                    {
                        i = await PushNotificationDeliveryManager.SendNextReadyNotificationAsync(ct);
                    }
                });

        }
    }
}
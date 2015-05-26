using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using TicketDesk.IO;

namespace TicketDesk.PushNotifications.Job
{
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            var demoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
            var isEnabled = false;
            var interval = 2;
            using (var context = new TdPushNotificationContext())
            {
                isEnabled = !demoMode && context.TicketDeskPushNotificationSettings.IsEnabled;
                interval = context.TicketDeskPushNotificationSettings.DeliveryIntervalMinutes;
            }
            var storageConnectionString = AzureConnectionHelper.CloudConfigConnString ??
                                             AzureConnectionHelper.ConfigManagerConnString;
            var host = new JobHost(new JobHostConfiguration(storageConnectionString));
            if (isEnabled)
            {
                host.Call(typeof(Functions).GetMethod("StartNotificationScheduler"), new { interval});
                host.RunAndBlock();
            }
            else
            {
                Console.Out.WriteLine("Push notifications are disabled");
                host.RunAndBlock();//just run and block, to keep from recycling the service over and over
            }


        }
    }
}

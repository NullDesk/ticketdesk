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

using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<TdPushNotificationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskPushNotifications";
        }

        protected override void Seed(TdPushNotificationContext context)
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"];
            if (!string.IsNullOrEmpty(demoMode) && demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                DemoPushNotificationDataManager.SetupDemoPushNotificationData(context);
            }
            else
            {
                if (!context.SubscriberPushNotificationSettings.Any(s => s.SubscriberId == "64165817-9cb5-472f-8bfb-6a35ca54be6a"))
                {
                    context.SubscriberPushNotificationSettings.Add(new SubscriberPushNotificationSetting()
                    {
                        SubscriberId = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                        IsEnabled = true,
                        PushNotificationDestinations = new PushNotificationDestinationCollection()
                        {
                            new PushNotificationDestination()
                            {
                                SubscriberName = "Admin User",
                                DestinationAddress = "admin@example.com",
                                DestinationType = "email"
                            }
                        }
                    });
                }
            }
            base.Seed(context);
        }
    }
}

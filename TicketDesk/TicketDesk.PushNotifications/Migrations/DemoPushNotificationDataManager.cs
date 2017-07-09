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

using System.Linq;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Migrations
{
    public static class DemoPushNotificationDataManager
    {
        public static void RemoveAllPushNotificationData(TdPushNotificationContext context)
        {
            context.TicketPushNotificationItems.RemoveRange(context.TicketPushNotificationItems);
            context.PushNotificationDestinations.RemoveRange(context.PushNotificationDestinations);
            context.SubscriberPushNotificationSettings.RemoveRange(context.SubscriberPushNotificationSettings);

            //context.PushNotificationItems.RemoveRange(context.PushNotificationItems);
            context.TicketDeskPushNotificationSettings = new ApplicationPushNotificationSetting();

            context.SaveChanges();


        }
        public static void SetupDemoPushNotificationData(TdPushNotificationContext context)
        {
            if (!context.SubscriberPushNotificationSettings.Any(s => 
                    s.SubscriberId == "64165817-9cb5-472f-8bfb-6a35ca54be6a"
                 || s.PushNotificationDestinations.Any(d => 
                                   d.DestinationAddress == "admin@example.com"
                                && d.DestinationType == "email")))
            {
                context.SubscriberPushNotificationSettings.Add(new SubscriberNotificationSetting()
                {
                    SubscriberId = "64165817-9cb5-472f-8bfb-6a35ca54be6a",
                    IsEnabled = true,
                    PushNotificationDestinations = new[]
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
            if (!context.SubscriberPushNotificationSettings.Any(s => 
                        s.SubscriberId == "72bdddfb-805a-4883-94b9-aa494f5f52dc"
                     || s.PushNotificationDestinations.Any(d =>
                            d.DestinationAddress == "staff@example.com"
                         && d.DestinationType == "email")))
            {
                context.SubscriberPushNotificationSettings.Add(new SubscriberNotificationSetting()
                {
                    SubscriberId = "72bdddfb-805a-4883-94b9-aa494f5f52dc",
                    IsEnabled = true,
                    PushNotificationDestinations = new[]
                    {
                        new PushNotificationDestination()
                        {
                            SubscriberName = "HelpDesk User",
                            DestinationAddress = "staff@example.com",
                            DestinationType = "email"
                        }
                    }
                });
            }

            if (!context.SubscriberPushNotificationSettings.Any(s => 
                    s.SubscriberId == "17f78f38-fa68-445f-90de-38896140db28"
                 || s.PushNotificationDestinations.Any(d =>
                           d.DestinationAddress == "user@example.com"
                        && d.DestinationType == "email")))
            {
                context.SubscriberPushNotificationSettings.Add(new SubscriberNotificationSetting()
                {
                    SubscriberId = "17f78f38-fa68-445f-90de-38896140db28",
                    IsEnabled = true,
                    PushNotificationDestinations = new[]
                    {
                        new PushNotificationDestination()
                        {
                            SubscriberName = "Regular User",
                            DestinationAddress = "user@example.com",
                            DestinationType = "email"
                        }
                    }
                });
            }
            context.SaveChanges();
        }
    }
}

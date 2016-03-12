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
using System.Configuration;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Migrations;
using TicketDesk.Web.Client.Infrastructure;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public static void ConfigurePushNotifications()
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false";

            if (!DatabaseConfig.IsDatabaseReady || demoMode.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                //disable if database hasn't been created, of if running in demo mode
                return;
            }

            //configuration supplied by the IoC configuration
            var context = DependencyResolver.Current.GetService<TdPushNotificationContext>();

            if (DatabaseConfig.IsFirstRunDemoRefreshEnabled())
            {
                DemoPushNotificationDataManager.SetupDemoPushNotificationData(context);
            }

            if (context.TicketDeskPushNotificationSettings.IsEnabled)
            {
                //TODO: poor man's detection of appropriate scheduler
                var siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
                var isAzureWebSite = !string.IsNullOrEmpty(siteName);
                if (!isAzureWebSite)
                {
                    InProcessPushNotificationScheduler.Start(context.TicketDeskPushNotificationSettings.DeliveryIntervalMinutes);
                }
                context.Dispose();//ensure that no one accidentally holds a reference to this in closure

                //register for static notifications created event handler 
                TdDomainContext.NotificationsCreated += (sender, notifications) =>
                {
                    // ReSharper disable once EmptyGeneralCatchClause
                    try
                    {
                        var notes = notifications as TicketEventNotification[] ?? notifications.ToArray();
                        if (notes.Any())
                        {
                            HostingEnvironment.QueueBackgroundWorkItem(
                                async ct =>
                                {
                                    var noteContext = DependencyResolver.Current.GetService<TdPushNotificationContext>();
                                    var subscriberExclude =
                                        noteContext.TicketDeskPushNotificationSettings.AntiNoiseSettings.ExcludeSubscriberEvents;
                                    await noteContext.AddNotificationsAsync(notes.ToNotificationEventInfoCollection(subscriberExclude));
                                    
                                    await noteContext.SaveChangesAsync(ct);
                                    
                                });
                        }
                    }
                    catch
                    {
                        //TODO: Log this somewhere
                    }
                };
            }
        }
        
    }
}
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
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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
        /// <summary>
        /// Configures the push notifications.
        /// </summary>
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

                if (context.TicketDeskPushNotificationSettings.IsBackgroundQueueEnabled)
                {
                    //register for static notifications created event handler 
                    TdDomainContext.NotificationsCreated += (sender, notifications) =>
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(CreateNotifications(notifications));
                    };
                }
                else
                {
                    TdDomainContext.NotificationsCreated += (sender, notifications) =>
                    {
                        CreateNotifications(notifications)(CancellationToken.None);
                    };
                }
                context.Dispose();//ensure that no one accidentally holds a reference to this in closure

            }
        }

        private static Action<CancellationToken> CreateNotifications(IEnumerable<TicketEventNotification> notifications)
        {
            return ct =>
            {
                // ReSharper disable once EmptyGeneralCatchClause
                try
                {
                    var notificationIds = notifications.Select(n => n.EventId).ToArray();
                    var domainContext = DependencyResolver.Current.GetService<TdDomainContext>();
                    var multiProject = domainContext.Projects.Count() > 1;
                    //fetch these back and make sure all dependent entities we need are loaded
                    var notes = domainContext.TicketEventNotifications
                        .Include(t => t.TicketEvent)
                        .Include(t => t.TicketEvent.Ticket)
                        .Include(t => t.TicketEvent.Ticket.Project)
                        .Include(t => t.TicketSubscriber)
                        .Where(t => notificationIds.Contains(t.EventId))
                        .ToArray();

                    if (notes.Any())
                    {
                        using (var noteContext = new TdPushNotificationContext())
                        {
                            var subscriberExclude =
                                noteContext.TicketDeskPushNotificationSettings.AntiNoiseSettings
                                    .ExcludeSubscriberEvents;

                            var noteEvents = notes.ToNotificationEventInfoCollection(subscriberExclude,
                                multiProject);

                            noteContext.AddNotifications(noteEvents);

                            noteContext.SaveChanges();
                        }
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
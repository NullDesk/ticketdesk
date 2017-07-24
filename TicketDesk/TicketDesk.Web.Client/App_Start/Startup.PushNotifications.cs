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
using TicketDesk.PushNotifications.Model;
using TicketDesk.Web.Client.Infrastructure;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        /// <summary>
        ///     Configures the push notifications.
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
                EnsureBroadcastNotificaitonsConfiguration(context);

                //TODO: poor man's detection of appropriate scheduler
                var siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
                var isAzureWebSite = !string.IsNullOrEmpty(siteName);
                if (!isAzureWebSite)
                {
                    InProcessPushNotificationScheduler.Start(context.TicketDeskPushNotificationSettings
                        .DeliveryIntervalMinutes);
                }

                if (context.TicketDeskPushNotificationSettings.IsBackgroundQueueEnabled)
                {
                    //register for static notifications created event handler 
                    TdDomainContext.NotificationsCreated += (sender, notifications) =>
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(CreateTicketEventNotifications(notifications));
                    };

                    TdDomainContext.TicketsCreated += (sender, tickets) =>
                    {
                        HostingEnvironment.QueueBackgroundWorkItem(CreateNewTicketBroadcastNotification(tickets));
                    };
                }
                else
                {
                    TdDomainContext.NotificationsCreated += (sender, notifications) =>
                    {
                        CreateTicketEventNotifications(notifications)(CancellationToken.None);
                    };
                    TdDomainContext.TicketsCreated += (sender, tickets) =>
                    {
                        CreateNewTicketBroadcastNotification(tickets)(CancellationToken.None);
                    };
                }
            }
            context.Dispose(); //ensure that no one accidentally holds a reference to this in closure
        }

        private static void EnsureBroadcastNotificaitonsConfiguration(TdPushNotificationContext context)
        {
            var broadcastUserSettings = context.SubscriberPushNotificationSettingsManager
                .GetSettingsForSubscriberAsync("new ticket broadcast").Result;

            var broadcastAppSettings = context.TicketDeskPushNotificationSettings.BroadcastSettings;
            if (broadcastUserSettings == null)
            {
                broadcastUserSettings = new SubscriberNotificationSetting
                {
                    SubscriberId = "new ticket broadcast"
                };
                context.SubscriberPushNotificationSettingsManager.AddSettingsForSubscriber(broadcastUserSettings);
            }
            if
            (
                broadcastAppSettings.BroadcastMode ==
                ApplicationPushNotificationSetting.PushNotificationBroadcastMode.CustomAddress
                &&
                !string.IsNullOrEmpty(broadcastAppSettings.SendToCustomEmailAddress)
            )
            {
                broadcastUserSettings.PushNotificationDestinations.Add(
                    new PushNotificationDestination
                    {
                        SubscriberId = "new ticket broadcast",
                        DestinationAddress = broadcastAppSettings.SendToCustomEmailAddress,
                        DestinationType = "email",
                        SubscriberName = broadcastAppSettings.SendToCustomEmailDisplayName
                    }
                );
            }
            else
            {


                var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
                var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
                var usersForNotification = roleManager.GetTdHelpDeskUsers(userManager)
                    .Union(roleManager.GetTdTdAdministrators(userManager))
                    .Distinct(new UniqueNameEmailDisplayUserEqualityComparer()).ToArray();


                var existingDestinations = broadcastUserSettings
                    .PushNotificationDestinations
                    .Where(pnd => pnd.DestinationType == "email" && pnd.SubscriberId == "new ticket broadcast")
                    .ToArray();
                //remove users not in list anymore
                var usersToRemove = existingDestinations
                    .Where(us => !usersForNotification
                        .Any(un => un.Email == us.DestinationAddress && un.DisplayName == us.SubscriberName))
                    .ToList();
                foreach (var us in usersToRemove)
                {
                    broadcastUserSettings.PushNotificationDestinations.Remove(us);
                }

                //add users in list, but not already in destinations
                foreach (var nu in usersForNotification)
                {
                    if (!existingDestinations.Any(ed => nu.Email == ed.DestinationAddress &&
                                                        nu.DisplayName == ed.SubscriberName))
                    {
                        broadcastUserSettings.PushNotificationDestinations.Add(
                            new PushNotificationDestination
                            {
                                SubscriberId = "new ticket broadcast",
                                DestinationAddress = nu.Email,
                                DestinationType = "email",
                                SubscriberName = nu.DisplayName
                            }
                        );
                    }
                }
            }
            broadcastUserSettings.IsEnabled = broadcastAppSettings.IsBroadcastEnabled;
            context.SaveChanges();
        }


        private static Action<CancellationToken> CreateNewTicketBroadcastNotification(IEnumerable<Ticket> tickets)
        {
            return ct =>
            {
                try
                {
                    var notificationIds = tickets.Select(t =>
                            t.TicketEvents.First(te => te.ForActivity == TicketActivity.Create ||
                                                       te.ForActivity == TicketActivity.CreateOnBehalfOf).EventId)
                        .ToArray();

                    var domainContext = DependencyResolver.Current.GetService<TdDomainContext>();
                    var multiProject = domainContext.Projects.Count() > 1;
                    var notes = domainContext.TicketEventNotifications
                        .Include(t => t.TicketEvent)
                        .Include(t => t.TicketEvent.Ticket)
                        .Include(t => t.TicketEvent.Ticket.Project)
                        .Where(t => notificationIds.Contains(t.EventId))
                        .ToArray();

                    if (notes.Any())
                    {
                        using (var noteContext = new TdPushNotificationContext())
                        {
                            var newNoteEvents = notes.ToNewTicketPushNotificationInfoCollection(multiProject);
                            noteContext.AddNotifications(newNoteEvents);

                            noteContext.SaveChanges();
                        }
                    }
                }
                catch
                {
                    //TODO: log this somewhere                    
                }
            };
        }

        private static Action<CancellationToken> CreateTicketEventNotifications(
            IEnumerable<TicketEventNotification> notifications)
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
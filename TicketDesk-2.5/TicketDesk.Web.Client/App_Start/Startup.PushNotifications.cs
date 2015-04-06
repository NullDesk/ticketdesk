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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications.Azure;
using TicketDesk.PushNotifications.Common;
using TicketDesk.PushNotifications.WebLocal;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public static void ConfigurePushNotifications()
        {
            var demoMode = ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false";

            if (!DatabaseConfig.IsDatabaseReady || demoMode.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                //disable if database hasn't been created, of if running in demo mode
                return;
            }
            //configure providers first!
            TdPushNotificationContext.Configure(GetPushNotificationProviders);
            var context = DependencyResolver.Current.GetService<TdPushNotificationContext>();
           
            if (context.PushNotificationSettings.IsEnabled)
            {
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
                                        noteContext.PushNotificationSettings.AntiNoiseSettings.ExcludeSubscriberEvents;
                                    await noteContext.AddNotifications(notes.ToNotificationEventInfoCollection(subscriberExclude));
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



        /// <summary>
        /// Gets the notification configuration.
        /// </summary>
        /// <returns>IEnumerable&lt;IPushNotifcationProvider&gt;.</returns>
        private static IEnumerable<IPushNotificationProvider> GetPushNotificationProviders()
        {
            //TODO: when we move to a plug-in model, this should be refactored to use an application setting.
            var potentialProviders = new List<IPushNotificationProvider>()
            {
                //TODO: for now, just new up one of each possible provider type and we'll pick what is correctly configured
                new AzurePushNotificationProvider(),
            };

            var pot = potentialProviders.Where(p => p.IsConfigured).ToArray();
            //web local is the last-ditch fallback provider, only use if/when no other providers are available
            return pot.Any() ? pot : new[] { new WebLocalPushNotificationProvider() };
        }

    }
}
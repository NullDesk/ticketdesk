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

using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Notifications.Azure;
using TicketDesk.Notifications.Common;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {

        public void ConfigureNotifications()
        {
            return;//TODO: Enable when providers are complete
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            if (context.TicketDeskSettings.PushNotificationSettings.IsEnabled)
            {
                TicketDeskNotificationContext.Configure(GetNotificationConfiguration);
                //register for static notifications created event handler 
                TicketDeskContext.NotificationsCreated += (sender, notifications) =>
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
                                    foreach (var p in TicketDeskNotificationContext.Current.NotifcationProviders)
                                    {
                                        await
                                            p.AddPendingNotifications(notes.ToNotificationItemItems())
                                                .ConfigureAwait(false);
                                    }
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
        /// Gets a notification configuration by scanning available providers in order of preference
        /// and taking all available.
        /// </summary>
        /// <returns>SearchContextConfiguration.</returns>
        /// <remarks>This is supplied to the search system as a func and invoked when needed.</remarks>
        private NotificaitonContextConfiguration GetNotificationConfiguration()
        {
            //TODO: when we move to a plug-in model, this should be refactored to use an application setting.
            var potentialProviders = new List<INotifcationProvider>()
            {
                //TODO: for now, just new up one of each possible provider type and we'll pick all that are correctly configured
                new AzureNotificationProvider()
            };

            var config = new NotificaitonContextConfiguration(potentialProviders.Where(p => p.IsConfigured).ToArray());
                
            return config;
        }

    }
}
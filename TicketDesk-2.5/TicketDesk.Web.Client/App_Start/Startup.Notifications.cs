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
using TicketDesk.PushNotifications.Azure;
using TicketDesk.PushNotifications.Common;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {

        public void ConfigureNotifications()
        {
            var context = DependencyResolver.Current.GetService<TdContext>();
            if (context.TicketDeskSettings.PushNotificationSettings.IsEnabled)
            {
                TdPushNotificationContext.Configure(GetNotificationConfiguration);
                //register for static notifications created event handler 
                TdContext.NotificationsCreated += (sender, notifications) =>
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
                                    await
                                        noteContext.AddPendingNotifications(notes.ToNotificationItemItems())
                                            .ConfigureAwait(false);
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
        private IEnumerable<IPushNotificationProvider> GetNotificationConfiguration()
        {
            //TODO: when we move to a plug-in model, this should be refactored to use an application setting.
            var potentialProviders = new List<IPushNotificationProvider>()
            {
                //TODO: for now, just new up one of each possible provider type and we'll pick all that are correctly configured
                new AzurePushNotificationProvider()
            };

            return potentialProviders.Where(p => p.IsConfigured);
        }

    }
}
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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Common.Model;

namespace TicketDesk.PushNotifications.Common
{

    public sealed class TdPushNotificationContext : DbContext
    {
        private static Func<IEnumerable<IPushNotificationProvider>> GetProvidersFunc { get; set; }
        public static void Configure(Func<IEnumerable<IPushNotificationProvider>> getProvidersFunc)
        {
            GetProvidersFunc = getProvidersFunc;
        }

        private static IEnumerable<IPushNotificationProvider> _pushNotifcationProviders;
        private static IEnumerable<IPushNotificationProvider> PushNotificationProviders
        {
            get { return _pushNotifcationProviders ?? (_pushNotifcationProviders = GetProvidersFunc()); }
        }


        public TdPushNotificationContext()
            : base("name=TicketDesk")
        {
            if (!PushNotificationProviders.Any())
            {
                throw new ConfigurationErrorsException("Cannot create TicketDeskNotificationContext, at least one push notification provider must be configured");
            }
            PushNotificationItems = Set<PushNotificationItem>();// dbset is internal, must manually initialize it for use by EF
        }

        
        internal DbSet<PushNotificationItem> PushNotificationItems { get; set; }

        public async Task<bool> AddPendingNotifications(IEnumerable<PushNotificationItem> items)
        {
            
            foreach (var item in items)
            {
                var citem = item;//closure foreach workaround
                var existingItem =
                    await
                        PushNotificationItems.SingleOrDefaultAsync( n =>
                                n.TicketId == citem.TicketId &&
                                n.SubscriberId == citem.SubscriberId &&
                                citem.DeliveryStatus == PushNotificationItemStatus.Scheduled);

                if (existingItem != null)
                {
                    //entry already exists, add the new item's event id(s) to the existing item's list of events
                    existingItem.TicketEvents = existingItem.TicketEvents.Union(item.TicketEvents).ToArray();


                    //TODO: check logic for anti-noise system and reset the delivery time. 
                    //  Since we have to inspect the main domain's ticketevent before sending (for read 
                    //      flags and such), no reason not to query domain for subscriber's settings too, but
                    //      it may be more effective to store those settings for faster lookup with the notification item anyway...
                
                
                }
                else
                {
                    //doesn't exist, so add it now
                    PushNotificationItems.Add(item);
                }
            }
            
           
            return true;
        }

    }
}

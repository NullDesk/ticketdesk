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
using TicketDesk.PushNotifications.Common.Model.Extensions;

namespace TicketDesk.PushNotifications.Common
{

    public sealed class TdPushNotificationContext : DbContext
    {
        private static Func<IEnumerable<IPushNotificationProvider>> GetProvidersFunc { get; set; }
        public static void Configure(Func<IEnumerable<IPushNotificationProvider>> getProvidersFunc)
        {
            GetProvidersFunc = getProvidersFunc;
        }

        private static IEnumerable<IPushNotificationProvider> _pushNotificationProviders;
        private static IEnumerable<IPushNotificationProvider> PushNotificationProviders
        {
            get { return _pushNotificationProviders ?? (_pushNotificationProviders = GetProvidersFunc()); }
        }


        public TdPushNotificationContext()
            : base("name=TicketDesk")
        {
            if (!PushNotificationProviders.Any())
            {
                throw new ConfigurationErrorsException("Cannot create TicketDeskNotificationContext, at least one push notification provider must be configured");
            }
            // dbsets are internal, must manually initialize it for use by EF
            PushNotificationItems = Set<PushNotificationItem>();
            ApplicationPushNotificationSettings = Set<ApplicationPushNotificationSetting>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationPushNotificationSetting>()
             .Property(p => p.Serialized)
             .HasColumnName("PushNotificationSettingsJson");

            modelBuilder.Entity<UserPushNotificationSetting>()
             .Property(p => p.Serialized)
             .HasColumnName("PushNotificationSettingsJson");
        }

        internal DbSet<PushNotificationItem> PushNotificationItems { get; set; }
        internal DbSet<ApplicationPushNotificationSetting> ApplicationPushNotificationSettings { get; set; }
        public DbSet<UserPushNotificationSetting> UserPushNotificationSettings { get; set; }


        public ApplicationPushNotificationSetting PushNotificationSettings
        {
            //TODO: these change infrequently, cache these
            get { return ApplicationPushNotificationSettings.GetTicketDeskSettings(); }
            set
            {
                var oldSettings = ApplicationPushNotificationSettings.GetTicketDeskSettings();
                if (oldSettings != null)
                {
                    ApplicationPushNotificationSettings.Remove(oldSettings);
                }
                ApplicationPushNotificationSettings.Add(value);
            }
        }

        public async Task<bool> AddNotifications(IEnumerable<PushNotificationEventInfo> infoItems)
        {

            foreach (var item in infoItems)
            {
                var citem = item;//foreach closure workaround
                var userSettings = UserPushNotificationSettings.GetSettingsForUser(citem.SubscriberId);
                var appSettings = PushNotificationSettings;
                var existingItem =
                    await
                        PushNotificationItems.SingleOrDefaultAsync(n =>
                                n.TicketId == citem.TicketId &&
                                n.SubscriberId == citem.SubscriberId &&
                                n.DeliveryStatus == PushNotificationItemStatus.Scheduled);

                if (existingItem != null)
                {
                    existingItem.AddNewEvent(citem, appSettings, userSettings);
                }
                else
                {
                    var newNote = citem.ToPushNotificationItem(appSettings, userSettings);
                    if (newNote.TicketEventsList.Any())
                    {
                        //only add the new note if it contains an event... if only CanceledEvents exist 
                        //  for new note, then the note came in pre-canceled by the sender. This happens 
                        //  when the ticket event is marked as read from the start --usually when a 
                        //  subscriber has initiated the event in the first place and anti-noise is set
                        //  to exclude suscriber's own events
                        PushNotificationItems.Add(newNote);
                    }
                }
            }


            return true;
        }



    }
}

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
            // dbsets are internal, must manually initialize for use by EF
            PushNotificationItems = Set<PushNotificationItem>();
            ApplicationPushNotificationSettings = Set<ApplicationPushNotificationSetting>();
            SubscriberPushNotificationSettings = Set<SubscriberPushNotificationSetting>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationPushNotificationSetting>()
             .Property(p => p.Serialized)
             .HasColumnName("PushNotificationSettingsJson");

            modelBuilder.ComplexType<PushNotificationDestinationCollection>()
             .Property(p => p.Serialized)
             .HasColumnName("PushNotificationDestinationsJson");

        }

        //This DbSet is managed entirely within this assembly, marking internal
        internal DbSet<PushNotificationItem> PushNotificationItems { get; set; }

        //These DbSets contain json serialized content. Callers cannot use standard LINQ to Entities 
        //  expressions with these safely. Marking internal to prevent callers having direct access
        //  We'll provide a thin layer of abstraction for safely handling external interactions instead. 
        internal DbSet<ApplicationPushNotificationSetting> ApplicationPushNotificationSettings { get; set; }
        internal DbSet<SubscriberPushNotificationSetting> SubscriberPushNotificationSettings { get; set; }

        private SubscriberPushNotificationSettingsManager _subscriberPushNotificationSettingsManager;
        public SubscriberPushNotificationSettingsManager SubscriberPushNotificationSettingsManager
        {
            get {
                return _subscriberPushNotificationSettingsManager ??
                       (_subscriberPushNotificationSettingsManager = new SubscriberPushNotificationSettingsManager(this));
            }
        }

        public ApplicationPushNotificationSetting TicketDeskPushNotificationSettings
        {
            //TODO: these change infrequently, cache these
            get
            {
                var apn = ApplicationPushNotificationSettings.GetTicketDeskSettings();
                //this should only ever happen once, but if no settings are in DB, make default set
                if (apn == null)
                {
                    apn = new ApplicationPushNotificationSetting();
                    //do this on another instance, because we don't want to commit other things that may be tracking on this instance
                    using (var tempContext = new TdPushNotificationContext())//this feels like cheating :)
                    {
                        ApplicationPushNotificationSettings.Add(apn);
                        tempContext.SaveChanges();
                    }
                    
                }
                return apn;
            }
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
                var userSettings = SubscriberPushNotificationSettings.GetSettingsForUser(citem.SubscriberId);
                var appSettings = TicketDeskPushNotificationSettings;
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
                        //  to exclude subscriber's own events
                        PushNotificationItems.Add(newNote);
                    }
                }
            }


            return true;
        }



    }
}

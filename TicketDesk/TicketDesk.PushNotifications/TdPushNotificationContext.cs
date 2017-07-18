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

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TicketDesk.PushNotifications.Model;
using TicketDesk.PushNotifications.Model.Extensions;

namespace TicketDesk.PushNotifications
{
    public sealed class TdPushNotificationContext : DbContext
    {
        public TdPushNotificationContext()
            : base("name=TicketDesk")
        {
         
            // dbsets are internal, must manually initialize for use by EF
            PushNotificationItems = Set<PushNotificationItem>();
            ApplicationPushNotificationSettings = Set<ApplicationPushNotificationSetting>();
            SubscriberPushNotificationSettings = Set<SubscriberNotificationSetting>();
            TicketPushNotificationItems = Set<TicketPushNotificationItem>();
            PushNotificationDestinations = Set<PushNotificationDestination>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationPushNotificationSetting>()
             .Property(p => p.Serialized)
             .HasColumnName("PushNotificationSettingsJson");

          
        }

        //This DbSet is managed entirely within this assembly, marking internal
        internal DbSet<PushNotificationItem> PushNotificationItems { get; set; }

        internal DbSet<TicketPushNotificationItem> TicketPushNotificationItems { get; set; }

        internal DbSet<PushNotificationDestination> PushNotificationDestinations { get; set; }

        //These DbSets contain json serialized content. Callers cannot use standard LINQ to Entities 
        //  expressions with these safely. Marking internal to prevent callers having direct access
        //  We'll provide a thin layer of abstraction for safely handling external interactions instead. 
        internal DbSet<ApplicationPushNotificationSetting> ApplicationPushNotificationSettings { get; set; }
        internal DbSet<SubscriberNotificationSetting> SubscriberPushNotificationSettings { get; set; }

        private SubscriberPushNotificationSettingsManager _subscriberPushNotificationSettingsManager;
        public SubscriberPushNotificationSettingsManager SubscriberPushNotificationSettingsManager
        {
            get
            {
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
                    var napn = new ApplicationPushNotificationSetting();
                    //do this on another instance, because we don't want to commit other things that may be tracking on this instance
                    using (var tempContext = new TdPushNotificationContext())//this feels like cheating :)
                    {
                        tempContext.ApplicationPushNotificationSettings.Add(napn);
                        tempContext.SaveChanges();
                    }
                    //try it again
                    apn = ApplicationPushNotificationSettings.GetTicketDeskSettings();
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

        public bool AddNotifications(IEnumerable<NewTicketPushNotificationInfo> infoItems)
        {
            var userSettings = SubscriberPushNotificationSettings.GetSettingsForUser("new ticket broadcast");
            
            foreach (var item in infoItems)
            {
                PushNotificationItems.AddRange(item.ToPushNotificationItems(userSettings));
            }
            return true;
        }

        public bool AddNotifications(IEnumerable<TicketPushNotificationEventInfo> infoItems)
        {
            var appSettings = TicketDeskPushNotificationSettings;

            foreach (var item in infoItems)
            {
                var citem = item; //foreach closure workaround
                var userSettings = SubscriberPushNotificationSettings.GetSettingsForUser(citem.SubscriberId);
               
                //get items already in db that haven't been sent yet
                var existingItems =TicketPushNotificationItems
                    .Include(t => t.PushNotificationItem)
                    .Include(t => t.PushNotificationItem.Destination)
                    .Where(n =>
                        n.PushNotificationItem.ContentSourceId == citem.TicketId &&
                        n.PushNotificationItem.ContentSourceType == "ticket" &&
                        n.PushNotificationItem.SubscriberId == citem.SubscriberId &&
                        n.PushNotificationItem.DeliveryStatus == PushNotificationItemStatus.Scheduled);
                
                //for already scheduled, just add the new event
                foreach (var existingItem in existingItems)
                {
                    existingItem.AddNewEvent(citem, appSettings, userSettings);
                }

                //get note items for each destination
                var schedNotes = citem.ToPushNotificationItems(appSettings, userSettings);
                foreach (var schedNote in schedNotes)
                {
                    //if no item for this destination in existing scheduled items list, add a new note for that destination
                    if (existingItems.All(i => i.PushNotificationItem.DestinationId != schedNote.PushNotificationItem.DestinationId))
                    {
                        if (schedNote.TicketEventsList.Any())
                        {
                            //only add the new note if it contains an event... if only CanceledEvents exist 
                            //  for new note, then the note came in pre-canceled by the sender. This happens 
                            //  when the ticket event is marked as read from the start --usually when a 
                            //  subscriber has initiated the event in the first place and anti-noise is set
                            //  to exclude subscriber's own events
                            TicketPushNotificationItems.Add(schedNote);
                        }
                    }
                }
            }
            return true;
        }
    }
}

// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using TicketDesk.Domain.Model;
using System.Data.Entity;
using TicketDesk.Domain.Model.Extensions;
using TicketDesk.Domain.Search;


namespace TicketDesk.Domain
{


    public class TicketDeskContext : DbContext
    {
        public TicketDeskContext()
            : base("name=TicketDesk")
        {

        }

        #region EF model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: Remove along with supoprting class if unneeded
            //modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());

            modelBuilder.Entity<TicketComment>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<TicketComment>()
                .HasMany(e => e.TicketEventNotifications)
                .WithRequired(e => e.TicketComment)
                .HasForeignKey(e => new { e.TicketId, e.CommentId })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.ComplexType<UserTicketListSettingsCollection>()
                .Property(p => p.Serialized)
                .HasColumnName("ListSettingsJson");

        }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }
        public virtual DbSet<TicketEventNotification> TicketEventNotifications { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketTag> TicketTags { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }

        #endregion

        public SearchManager SearchManager
        {
            get
            {
                return SearchManager.GetInstance(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")));
            } 
        
        }

        #region utility
        public override int SaveChanges()
        {
            var changes = ChangeTracker.Entries<Ticket>().Select(t => t.Entity);
            var result = base.SaveChanges();
            // ReSharper disable once EmptyGeneralCatchClause
            try
            {
                if (result > 0)
                {
                    var queueItems = changes.ToSeachQueueItems();
                    var t = SearchManager.QueueItemsForIndexing(queueItems);
                    t.Wait();//wait so the thread doesn't exit before this finishes
                }
            }

            catch//eat the exception, we NEVER want this to interfere with the save operation
            {
                //TODO: Log this somewhere
            }
            return result;
        }

        public ObjectQuery<T> GetObjectQueryFor<T>(IDbSet<T> entity) where T : class
        {
            var oc = ((IObjectContextAdapter)this).ObjectContext;
            return oc.CreateObjectSet<T>();
        }

        private string GetSearchIndexLocation()
        {
            var indexLocation = Settings.GetSettingValue("SearchIndexLocation", (string)null);
            if (indexLocation == null)
            {
                var datadir = AppDomain.CurrentDomain.GetData("DataDirectory");
                indexLocation = datadir == null ? "ram" : Path.Combine(datadir.ToString(), "SearchIndexes");
            }
            return indexLocation;
        }

        #endregion
    }
}

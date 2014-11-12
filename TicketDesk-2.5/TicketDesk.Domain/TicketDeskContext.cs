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
using System.Threading.Tasks;
using Lucene.Net.Search;
using TicketDesk.Domain.Model;
using System.Data.Entity;
using TicketDesk.Domain.Search;


namespace TicketDesk.Domain
{
    public class TicketDeskContext : DbContext
    {
        public TicketDeskContextSecurityProviderBase SecurityProvider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketDeskContext"/> class.
        /// </summary>
        /// <remarks>
        /// The securityProvider parameter can be left null; however, this should 
        /// be reserved only for back-end and automated functionality that runs
        /// outside of a user's context (e.g. migrations)
        /// </remarks>
        /// <param name="securityProvider">The security provider.</param>
        public TicketDeskContext(TicketDeskContextSecurityProviderBase securityProvider)
            : this()
        {
            SecurityProvider = securityProvider;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TicketDeskContext"/> class.
        /// </summary>
        /// <remarks>
        /// Some functions related to migrations still expect to be able to construct the
        /// DbContext from a parameterless ctor. 
        /// 
        /// Initializers were fixed in EF 6.1 so they
        /// can be use the context from which they were called instead of constructing a new
        /// instance internally, but a few obscure bits were not similarly updates (e.g. 
        /// DbMigrator.GetPendingMigrations). 
        /// </remarks>
        internal TicketDeskContext()
            : base("name=TicketDesk")
        {

        }


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


        public ObjectQuery<T> GetObjectQueryFor<T>(IDbSet<T> entity) where T : class
        {
            var oc = ((IObjectContextAdapter)this).ObjectContext;
            return oc.CreateObjectSet<T>();
        }

        public SearchManager SearchManager
        {
            get
            {
                return SearchManager.GetInstance(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")));
            }
        }

        public override async Task<int> SaveChangesAsync()
        {

            //TODO: extract and consolidate with sync version of SaveChanges method
            // ReSharper disable PossibleMultipleEnumeration
            var ticketChanges = ChangeTracker.Entries<Ticket>().Select(t => t.Entity);

            foreach (var change in ticketChanges)
            {
                if (change.TicketId == default(int))
                {
                    PrePopulateNewTicket(change);
                }
            }

            var result = await base.SaveChangesAsync();
            // ReSharper disable once EmptyGeneralCatchClause
            try
            {
                //queue up for search index update
                if (result > 0)
                {

                    var queueItems = ticketChanges.ToSeachQueueItems();
                    //config await to resume on a new thread, not the context's thread... prevents deadlock on the UI thread
                    var task = SearchManager.QueueItemsForIndexingAsync(queueItems); //.ConfigureAwait(false);
                    task.RunSynchronously();
                }
            }
            catch
            {
                //TODO: Log this somewhere
            }
            // ReSharper restore PossibleMultipleEnumeration
            return result;
        }

        public override int SaveChanges()
        {
            // ReSharper disable PossibleMultipleEnumeration
            var ticketChanges = ChangeTracker.Entries<Ticket>().Select(t => t.Entity);

            foreach (var change in ticketChanges)
            {
                if (change.TicketId == default(int))
                {
                    PrePopulateNewTicket(change);
                }
            }
            var result = base.SaveChanges();

            // ReSharper disable once EmptyGeneralCatchClause
            try
            {
                //queue up for search index update
                if (result > 0)
                {

                    var queueItems = ticketChanges.ToSeachQueueItems();
                    //config await to resume on a new thread, not the context's thread... prevents deadlock on the UI thread
                    var task = SearchManager.QueueItemsForIndexingAsync(queueItems); //.ConfigureAwait(false);
                    task.RunSynchronously();
                }
            }
            catch
            {
                //TODO: Log this somewhere
            }
            // ReSharper restore PossibleMultipleEnumeration
            return result;
        }

        private void PrePopulateNewTicket(Ticket newTicket)
        {
            //TODO: Move this shit somewhere else

            //TODO: CheckUserSecurityForActivity

            //TODO: double check owner if populated, make sure submitter can set this field if it isn't their id already
            
            //TODO: double check assigned if populated, make sure submitter can set this field.

            var now = DateTime.Now;
            newTicket.Owner = newTicket.Owner ?? SecurityProvider.GetCurrentUserId();
            newTicket.CreatedBy = SecurityProvider.GetCurrentUserId();
            newTicket.CreatedDate = now;
            newTicket.TicketStatus = TicketStatus.Active;
            newTicket.CurrentStatusDate = now;
            newTicket.CurrentStatusSetBy = SecurityProvider.GetCurrentUserId();
            newTicket.LastUpdateBy = SecurityProvider.GetCurrentUserId();
            newTicket.LastUpdateDate = now;


            newTicket.TicketTags.AddRange(newTicket.TagList.Split(',').Select(tag =>
                    new TicketTag()
                    {
                        TagName = tag.Trim()
                    }));


            //TODO: Add opening comment

            
        }
    }
}

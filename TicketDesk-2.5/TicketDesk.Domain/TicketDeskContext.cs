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
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using TicketDesk.Domain.Annotations;
using TicketDesk.Domain.Localization;
using TicketDesk.Domain.Model;
using System.Data.Entity;


namespace TicketDesk.Domain
{
    public class TicketDeskContext : DbContext
    {

        /// <summary>
        /// Occurs when tickets have been changed and persisted to the database. 
        /// </summary>
        /// <remarks>
        /// WARNING: Do not to register events from instance objects unless you 
        /// are sure they will de-register when they go out of scope. The 
        /// registered delegate will keep a reference to the isntance that that 
        /// registered. You can cause memory leaks if you aren't careful with 
        /// static events.
        ///</remarks>
        public static event EventHandler<IEnumerable<Ticket>> TicketsChanged;

        private static void RaiseTicketsChanged(TicketDeskContext sender, IEnumerable<Ticket> tickets)
        {
            //TODO: Static events have their (rare) uses, but this should use a service bus or formal pub/sub mechanism eventually
            if (TicketsChanged != null)
            {
                TicketsChanged(sender, tickets);
            }
        }

        public TicketDeskContextSecurityProviderBase SecurityProvider { get; private set; }

        public TicketActionManager TicketActions { get; set; }


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
            TicketActions = TicketActionManager.GetInstance(SecurityProvider);
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
        /// instance internally, but a few obscure bits were not similarly updated (e.g. 
        /// DbMigrator.GetPendingMigrations). 
        /// </remarks>
        public TicketDeskContext()
            : base("name=TicketDesk")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.ComplexType<UserTicketListSettingsCollection>()
                .Property(p => p.Serialized)
                .HasColumnName("ListSettingsJson");

            modelBuilder.ComplexType<ApplicationSelectListSetting>()
                .Property(p => p.Serialized)
                .HasColumnName("SelectListSettingsJson");

            modelBuilder.ComplexType<ApplicationPermissionsSetting>()
                .Property(p => p.Serialized)
                .HasColumnName("PermissionsSettingsJson");
        }

        public virtual DbSet<TicketEvent> TicketEvents { get; [UsedImplicitly]set; }
        public virtual DbSet<Ticket> Tickets { get; [UsedImplicitly] set; }
        public virtual DbSet<TicketTag> TicketTags { get; [UsedImplicitly] set; }
        public virtual DbSet<UserSetting> UserSettings { get; [UsedImplicitly] set; }
        public virtual DbSet<TicketSubscriber> TicketSubscribers { get; set; }
        /// <summary>
        /// Use TicketDeskSettings for more convienient access to settings instead. Gets or sets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; [UsedImplicitly]set; }

        /// <summary>
        /// Gets or sets the application settings specific to ticketdesk.
        /// </summary>
        /// <value>The application settings.</value>
        public ApplicationSetting TicketDeskSettings
        {
            get { return ApplicationSettings.GetTicketDeskSettings(); }
            set
            {
                var oldSettings = ApplicationSettings.GetTicketDeskSettings();
                if (oldSettings != null)
                {
                    ApplicationSettings.Remove(oldSettings);
                }
                ApplicationSettings.Add(value);
            }
        }


        public ObjectQuery<T> GetObjectQueryFor<T>(IDbSet<T> entity) where T : class
        {
            var oc = ((IObjectContextAdapter)this).ObjectContext;
            return oc.CreateObjectSet<T>();
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());

            //skip the custom validation if a security provider wasn't supplied
            if (SecurityProvider != null && entityEntry.Entity is Ticket && entityEntry.State == EntityState.Added)
            {
                var ticket = entityEntry.Entity as Ticket;
                if (!TicketActions.IsTicketActivityValid(ticket, TicketActivity.Create))
                {
                    result.ValidationErrors.Add(new
                        DbValidationError("authorization",
                        TicketDeskDomainText.ExceptionSecurityUserCannotCreateNewTicket));
                }
            }
            return result.ValidationErrors.Count > 0 ? result : base.ValidateEntity(entityEntry, items);
        }

        public override async Task<int> SaveChangesAsync()
        {
            var pendingTicketChanges = GetTicketChanges().ToArray();
            if (SecurityProvider != null)
            {
                PreProcessNewTickets();
                PreProcessModifiedTickets(pendingTicketChanges);
            }
            var result = await base.SaveChangesAsync();
            if (result > 0)
            {
                RaiseTicketsChanged(this, pendingTicketChanges);
            }
            return result;
        }

        public override int SaveChanges()
        {
            var pendingTicketChanges = GetTicketChanges().ToArray();
            if (SecurityProvider != null)
            {
                PreProcessNewTickets();
                PreProcessModifiedTickets(pendingTicketChanges);
            }
            var result = base.SaveChanges();
            if (result > 0)
            {
                RaiseTicketsChanged(this, pendingTicketChanges);
            }
            return result;
        }

        private void PreProcessModifiedTickets(IEnumerable<Ticket> ticketChanges)
        {
            foreach (var change in ticketChanges)
            {
                PrePopulateModifiedTicket(change);
            }
        }

        private void PreProcessNewTickets()
        {
            var ticketChanges = ChangeTracker.Entries<Ticket>().Where(t => t.State == EntityState.Added).Select(t => t.Entity);

            foreach (var change in ticketChanges)
            {
                PrePopulateNewTicket(change);
            }
        }

        private void PrePopulateModifiedTicket(Ticket modifiedTicket)
        {
            var o = ChangeTracker.Entries<Ticket>().Single(e => e.Entity.TicketId == modifiedTicket.TicketId);
            var now = DateTime.Now;

            modifiedTicket.LastUpdateBy = SecurityProvider.CurrentUserId;
            modifiedTicket.LastUpdateDate = now;

            if (o.State != EntityState.Added)//can't access orig values for new entities
            {
                var origTicket = (Ticket)o.OriginalValues.ToObject();
                if (modifiedTicket.TicketStatus != origTicket.TicketStatus)
                //if status change, force update to status by/date
                {
                    modifiedTicket.CurrentStatusDate = now;
                    modifiedTicket.CurrentStatusSetBy = SecurityProvider.CurrentUserId;
                }
            }
        }

        private void PrePopulateNewTicket(Ticket newTicket)
        {
            //TODO: Move this somewhere else?

            //TODO: double check owner if populated, make sure submitter can set this field if it isn't their id already
            //TODO: double check assigned if populated, make sure submitter can set this field.

            var now = DateTime.Now;
            newTicket.Owner = newTicket.Owner ?? SecurityProvider.CurrentUserId;
            newTicket.CreatedBy = SecurityProvider.CurrentUserId;
            newTicket.CreatedDate = now;
            newTicket.TicketStatus = TicketStatus.Active;
            newTicket.CurrentStatusDate = now;
            newTicket.CurrentStatusSetBy = SecurityProvider.CurrentUserId;

            //last update info will be set by PrePopulateModifiedTicket method, no need to set it here too
            //newTicket.LastUpdateBy = SecurityProvider.CurrentUserId;
            //newTicket.LastUpdateDate = now;

            if (newTicket.TagList != null && newTicket.TagList.Any())
            {
                newTicket.TicketTags.AddRange(newTicket.TagList.Split(',').Select(tag =>
                    new TicketTag
                    {
                        TagName = tag.Trim()
                    }));
            }

            var act = (newTicket.Owner != SecurityProvider.CurrentUserId)
                ? TicketActivity.CreateOnBehalfOf
                : TicketActivity.Create;

            newTicket.TicketEvents.AddActivityEvent(
                SecurityProvider.CurrentUserId,
                act,
                null,
                null,
                SecurityProvider.GetUserDisplayName(newTicket.Owner));


            //TODO: What with attachments?
        }

        private IEnumerable<Ticket> GetTicketChanges()
        {
            var pendingCommentChanges =
                ChangeTracker.Entries<TicketEvent>().Where(t => t.State != EntityState.Unchanged)
                    .Select(t => t.Entity.TicketId).ToArray();

            var pendingTicketChanges = ChangeTracker.Entries<Ticket>()
                .Where(t => t.State != EntityState.Unchanged || pendingCommentChanges.Contains(t.Entity.TicketId))
                .Select(t => t.Entity)
                .ToArray(); //execute now, because after save changes this query will return no results
            return pendingTicketChanges;
        }
    }
}

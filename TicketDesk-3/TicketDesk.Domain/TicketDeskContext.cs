using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    public class TicketDeskContext : DbContext
    {

        public TicketDeskContext(string nameOrConnectionString) 
            : base(nameOrConnectionString) { }

        public TicketDeskContext()
            : base("TicketDesk")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketEventNotification> TicketEventNotifications { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketTag> TicketTags { get; set; }
        public DbSet<AdCachedRoleMember> AdCachedRoleMembers { get; set; }
        public DbSet<AdCachedUserProperty> AdCachedUserProperties { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}

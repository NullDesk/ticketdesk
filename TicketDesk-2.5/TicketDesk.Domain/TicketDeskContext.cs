using TicketDesk.Domain.Conventions;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TicketDeskContext : DbContext
    {
        public TicketDeskContext()
            : base("name=TicketDesk")
        {

        }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }
        public virtual DbSet<TicketEventNotification> TicketEventNotifications { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketTag> TicketTags { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());

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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;

using Microsoft.AspNet.Identity.EntityFramework;

using TicketDesk.Domain.Model;
//using TicketDesk.Domain.UnitOfWork;
//using TicketDesk.Data.Repositories;
//using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain
{
    public class TicketDeskContext : IdentityDbContext<UserProfile>
    {

        public TicketDeskContext() : base() { }

        public TicketDeskContext(string nameOrConnectionString): base(nameOrConnectionString) { }
       


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = false;
            
          
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


       
    }
}

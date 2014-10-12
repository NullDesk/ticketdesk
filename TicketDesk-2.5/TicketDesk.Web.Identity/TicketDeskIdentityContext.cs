using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public class TicketDeskIdentityContext : IdentityDbContext<TicketDeskUser>
    {
        public TicketDeskIdentityContext()
            : base("TicketDesk", throwIfV1Schema: true)
        {
        }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //we have to call the base modelbuilder first, or the new names get overridden by the base
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketDeskUser>().ToTable("IdentityUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("IdentityRoles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("IdentityUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("IdentityUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("IdentityUserClaims");

        }

        public string[] DefaultRoleNames
        {
            get { return TicketDeskIdentityContext.DefaultRoles; }
        }

        //TODO: convert to enum, and extend TdRoleManager to use roles via enum or string name
        public static string[] DefaultRoles
        {
            get
            {
                return new[]
                {
                    "TdAdministrators", 
                    "TdHelpDeskUsers", 
                    "TdInternalUsers", 
                    "TdPendingUsers"
                };
            }
        }
    }
}

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

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public class TicketDeskIdentityContext : IdentityDbContext<TicketDeskUser>
    {
        public TicketDeskIdentityContext()
            : base("TicketDesk", true)
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
            get { return DefaultRoles; }
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

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

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public class TdIdentityContext : IdentityDbContext<TicketDeskUser>
    {
        public TdIdentityContext()
            : base("TicketDesk", true)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //we have to call the base modelbuilder first, or the new names get overridden by the base
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketDeskUser>().ToTable("IdentityUsers");
            modelBuilder.Entity<TicketDeskRole>().ToTable("IdentityRoles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("IdentityUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("IdentityUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("IdentityUserClaims");

        }

        public static IEnumerable<string> DefaultRoleNames
        {
            get { return DefaultRoles.Select(r => r.Name); }
        }

        public static IEnumerable<TicketDeskRole> DefaultRoles
        {
            get
            {
                return new[]
                {
                    new TicketDeskRole
                    {
                        Name = "TdAdministrators",
                        DisplayName = "Administrators",
                        Description = "Can manage settings, users, and access application logs"
                    },
                     new TicketDeskRole
                    {
                        Name = "TdHelpDeskUsers",
                        DisplayName = "Help Desk Users",
                        Description = "May be assigned tickets, have full control over all existing tickets."

                     },
                     new TicketDeskRole
                    {
                        Name = "TdInternalUsers",
                        DisplayName = "Internal Users",
                        Description = "Can submit tickets and view all existing tickets."
                     },
                     new TicketDeskRole
                    {
                        Name = "TdPendingUsers",
                        DisplayName = "Pending Approval",
                        Description = "Users that have registered, but who's accounts have not been approved by an Administrator."
                     }
                };
            }
        }

    }
}

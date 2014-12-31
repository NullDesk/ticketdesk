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

using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public class TicketDeskRoleManager : RoleManager<IdentityRole>
    {
        public TicketDeskRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        

        ////TODO: Why are options passed here, and what are they for? This is from the Microsoft.AspNet.Identity.Samples package
        //public static TicketDeskRoleManager Create(IdentityFactoryOptions<TicketDeskRoleManager> options, IOwinContext context)
        //{
        //    return new TicketDeskRoleManager(new RoleStore<IdentityRole>(context.Get<TicketDeskIdentityContext>()));
        //}
       

        public IOrderedEnumerable<TicketDeskUser> GetTdInternalUsers(TicketDeskUserManager userManager)
        {
            return GetUsersInRole("TdInternalUsers", userManager);
        }
        public IOrderedEnumerable<TicketDeskUser> GetTdTdAdministrators(TicketDeskUserManager userManager)
        {
            return GetUsersInRole("", userManager);
        }

        public IOrderedEnumerable<TicketDeskUser> GetTdHelpDeskUsers(TicketDeskUserManager userManager)
        {
            return GetUsersInRole("TdHelpDeskUsers", userManager);
        }
        public IOrderedEnumerable<TicketDeskUser> GetTdPendingUsers(TicketDeskUserManager userManager)
        {
            return GetUsersInRole("TdPendingUsers", userManager);
        }

        public IOrderedEnumerable<TicketDeskUser> GetUsersInRole(string roleName, TicketDeskUserManager userManager)
        {
           return this
                .FindByName(roleName)
                .Users
                .GetUsersInRole(userManager)
                .OrderBy(u => u.DisplayName);
        }

    }
}
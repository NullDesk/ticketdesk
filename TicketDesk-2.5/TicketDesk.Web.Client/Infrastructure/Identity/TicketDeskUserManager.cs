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

using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{

    public class TicketDeskUserManager : UserManager<TicketDeskUser>
    {
        public TicketDeskUserManager(IUserStore<TicketDeskUser> store)
            : base(store)
        {

        }

        public UserDisplayInfoCache InfoCache { get { return new UserDisplayInfoCache(this); } }

        public bool IsTdHelpDeskUser(string userId)
        {
            return this.IsInRole(userId, "TdHelpDeskUsers");
        }

        public bool IsTdInternalUser(string userId)
        {
            return this.IsInRole(userId, "TdInternalUsers");
        }

        public bool IsTdAdministrator(string userId)
        {
            return this.IsInRole(userId, "TdAdministrators");
        }

        public bool IsTdPendingUser(string userId)
        {
            return this.IsInRole(userId, "TdPendingUsers");
        }

    }
}
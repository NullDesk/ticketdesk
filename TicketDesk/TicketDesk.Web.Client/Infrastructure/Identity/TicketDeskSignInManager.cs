// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public class TicketDeskSignInManager : SignInManager<TicketDeskUser, string>
    {
        public TicketDeskSignInManager(TicketDeskUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(TicketDeskUser user)
        {
            return user.GenerateUserIdentityAsync((TicketDeskUserManager)UserManager);
        }

        public static TicketDeskSignInManager Create(IdentityFactoryOptions<TicketDeskSignInManager> options, IOwinContext context)
        {
            return new TicketDeskSignInManager(context.GetUserManager<TicketDeskUserManager>(), context.Authentication);
        }
    }
}
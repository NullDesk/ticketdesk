using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
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
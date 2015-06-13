using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public static class TicketDeskUserManagerExtensions
    {
        public static UserDisplayInfo GetUserInfo(this TicketDeskUserManager userManager, string userId)
        {
            return userManager.InfoCache.GetUserInfo(userId) ?? new UserDisplayInfo();
        }
        public static void InitializeUserManager(this TicketDeskUserManager userManager, IAppBuilder app)
        {
            userManager.InitializeUserManager();

            //TODO: research DpapiDataProtectionProvider and figure out what the f*** this is supposed to do
            var dataProtectionProvider = app.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = new DataProtectorTokenProvider<TicketDeskUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
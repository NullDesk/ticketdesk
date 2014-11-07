using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public static class TicketDeskIdentityExtensions
    {
        public static IEnumerable<TicketDeskUser> GetUsersInRole(this ICollection<IdentityUserRole> roleUsers, UserManager<TicketDeskUser> userManager)
        {
            var ids = roleUsers.Select(u => u.UserId);
            return userManager.Users.Where(u => ids.Contains(u.Id));
        }

        public static string GetUserDisplayName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                return ci.FindFirstValue(ClaimTypes.GivenName);
            }
            return null;
        }
    }
}

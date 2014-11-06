using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public static class IIdentityExtensions
    {
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

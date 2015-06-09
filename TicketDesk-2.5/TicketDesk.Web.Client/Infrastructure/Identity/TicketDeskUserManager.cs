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

using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
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
        internal void InitializeUserManager(IAppBuilder app)
        {
            UserValidator = new UserValidator<TicketDeskUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            //TODO: research DpapiDataProtectionProvider and figure out what the f*** this is supposed to do
            var dataProtectionProvider = app.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<TicketDeskUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}
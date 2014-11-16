using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TicketDesk.Domain;
using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client
{
    public class TicketDeskContextSecurityProvider : TicketDeskContextSecurityProviderBase
    {
        private TicketDeskUserManager UserManager { get; set; }

        public TicketDeskContextSecurityProvider()
        {
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
        }

        protected override Func<string> GetCurrentUserId
        {
            get { return HttpContext.Current.User.Identity.GetUserId; }
        }

        protected override Func<string, bool> GetIsTdHelpDeskUser
        {
            get { return UserManager.IsTdHelpDeskUser; }
        }

        protected override Func<string, bool> GetIsTdInternalUser
        {
            get { return UserManager.IsTdInternalUser; }
        }

        protected override Func<string, bool> GetIsTdAdministrator
        {
            get { return UserManager.IsTdAdministrator; }
        }

        protected override Func<string, bool> GetIsTdPendingUser
        {
            get { return UserManager.IsTdPendingUser; }
        }

        public override Func<string, string> GetUserDisplayName
        {
            get { return (userId) => UserManager.InfoCache.GetUserInfo(userId).IfNotNull(i => i.DisplayName); }
        }
    }
}
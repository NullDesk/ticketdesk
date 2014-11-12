using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using Microsoft.AspNet.Identity;

namespace TicketDesk.Web.Client
{
     public class TicketDeskContextSecurityProvider : TicketDeskContextSecurityProviderBase
    {
        private TicketDeskUserManager UserManager { get; set; }

        public TicketDeskContextSecurityProvider()
        {
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
        }

        public override Func<string> GetCurrentUserId
        {
            get { return HttpContext.Current.User.Identity.GetUserId; }
        }

         public override Func<string, bool> GetIsTdHelpDeskUser
        {
            get { return UserManager.IsTdHelpDeskUser; }
        }

        public override Func<string, bool> GetIsTdInternalUser
        {
            get { return UserManager.IsTdInternalUser; }
        }

        public override Func<string, bool> GetIsTdAdministrator
        {
            get { return UserManager.IsTdAdministrator; }
        }

        public override Func<string, bool> GetIsTdPendingUser
        {
            get { return UserManager.IsTdPendingUser; }
        }
    }
}
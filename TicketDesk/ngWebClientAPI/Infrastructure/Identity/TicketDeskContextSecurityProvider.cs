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

using System;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace ngWebClientAPI
{
    [System.Web.Http.Authorize]
    public sealed class TicketDeskContextSecurityProvider : TdDomainSecurityProviderBase
    {
        private TicketDeskUserManager UserManager { get; set; }

        public TicketDeskContextSecurityProvider()
        {
            UserManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            string userName = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
          
            CurrentUser = new CPUUser(userName);
        }
        public override CPUUser CurrentUser { get; set; }


        protected override Func<CPUUser, bool> GetIsTdHelpDeskUser
        {
            get { return UserManager.IsTdHelpDeskUser; }
        }

        protected override Func<CPUUser, bool> GetIsTdInternalUser
        {
            get { return UserManager.IsTdInternalUser; }
        }

        protected override Func<CPUUser, bool> GetIsTdAdministrator
        {
            get { return UserManager.IsTdAdministrator; }
        }

        protected override Func<CPUUser, bool> GetIsTdPendingUser
        {
            get { return UserManager.IsTdPendingUser; }
        }

    }
}
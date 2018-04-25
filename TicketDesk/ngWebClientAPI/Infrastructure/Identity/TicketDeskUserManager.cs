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
//using Microsoft.AspNet.Identity;
//using TicketDesk.Web.Identity.Model;

namespace ngWebClientAPI
{
    public class TicketDeskUserManager
    {
        public bool IsTdHelpDeskUser(string userId)
        {
            bool IsTdHelpDeskUser = true;

            /*Need to change this method.*/
            return IsTdHelpDeskUser;
        }

        public bool IsTdInternalUser(string userId)
        {
            /*Need to change this method.*/
            bool IsTdInternalUser = true;
            return IsTdInternalUser;
        }

        public bool IsTdAdministrator(string userId)
        {
            /*Need to change this method.*/
            bool IsTdAministrator = true; ;
            return IsTdAministrator;
        }

        public bool IsTdPendingUser(string userId)
        {
            /*Need to change this method.*/
            bool IsTdPendingUser = false;
            return IsTdPendingUser;
        }
    }
}
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
using ngWebClientAPI.Models;
using TicketDesk.Domain.Model;
using System.Collections.Generic;

namespace ngWebClientAPI
{

    public class TicketDeskUserManager
    {
        public bool IsTdHelpDeskUser(CPUUser user)
        {
            bool IsTdHelpDeskUser = false;
            
            if(user.groups.Contains("TD_Resolver"))
            {
                IsTdHelpDeskUser = true;
            }
           
            return IsTdHelpDeskUser;
        }

        public bool IsTdInternalUser(CPUUser user)
        {

            /* This method needs to check if the CPU user is an active employee. 
             * Right now I did not find any way to check if the user was active 
             */

            bool IsTdInternalUser = true;
          
            return IsTdInternalUser;
        }

        public bool IsTdAdministrator(CPUUser user)
        {
            bool IsTdAministrator = false;

            if (user.groups.Contains("TD_Admin"))
            {
                IsTdAministrator = true;
            }

            return IsTdAministrator;
        }

        public bool IsTdPendingUser(CPUUser user)
        {
            /*  
             * This is a TicketDestk back-end attribute that exists
             * for security. I am not sure this will be necessary for CPU's purposes.
             */

            bool IsTdPendingUser = false;
            return IsTdPendingUser;
        }
    }
}
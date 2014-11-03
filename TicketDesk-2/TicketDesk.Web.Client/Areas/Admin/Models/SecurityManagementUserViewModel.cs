// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Areas.Admin.Models
{
    public class SecurityManagementUserViewModel : RegisterModel
    {


        [DisplayName("Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }
        
        [DisplayName("Is Staff")]
        public bool IsStaff { get; set; }

        [DisplayName("Is Submitter")]
        public bool IsSubmitter { get; set; }

        [DisplayName("Locked")]
        public bool IsLockedOut { get; set; }

    }
}
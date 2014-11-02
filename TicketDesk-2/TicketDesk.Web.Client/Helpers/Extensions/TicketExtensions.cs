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
using TicketDesk.Web.Client.Controllers;
using TicketDesk.Domain.Models;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Helpers
{
    public static class TicketExtensions
    {

        public static SelectListUtility selectUtility
        {
            get
            {
                var util = MefHttpApplication.ApplicationContainer.GetExportedValue<SelectListUtility>();
                return util;
            }
        }


        public static string GetCurrentUserName(this ApplicationController controller)
        {
            return controller.Security.CurrentUserName;
        }

        public static string GetCreatedByDisplayName(this Ticket ticket, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(ticket.CreatedBy);
        }

        public static string GetOwnerDisplayName(this Ticket ticket, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(ticket.Owner);
        }

        public static string GetAssignedToDisplayName(this Ticket ticket, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(ticket.AssignedTo);
        }

        public static string GetCurrentStatusByDisplayName(this Ticket ticket, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(ticket.CurrentStatusSetBy);
        }

        public static string GetLastUpdateByDisplayName(this Ticket ticket, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(ticket.LastUpdateBy);
        }


        public static string GetCommentByDisplayName(this TicketComment comment, ApplicationController controller)
        {
            return controller.Security.GetUserDisplayName(comment.CommentedBy);
        }



        public static SelectList GetPrioritySelectList(this Ticket ticket)
        {

            return selectUtility.GetPriorityList(true, ticket.Priority);
        }

        public static SelectList GetAssignToSelectList(this Ticket ticket, string currentUserName)
        {


            return selectUtility.GetStaffList(true, string.Empty, ticket.AssignedTo, currentUserName);
        }

        public static SelectList GetOwnerSelectList(this Ticket ticket)
        {
             var includeEmpty = string.IsNullOrEmpty(ticket.Owner);

             return selectUtility.GetSubmittersList(includeEmpty, ticket.Owner);
        }

        public static SelectList GetTicketTypeSelectList(this Ticket ticket)
        {
            return selectUtility.GetTicketTypeList(false, ticket.Type);
        }


        public static SelectList GetCategorySelectList(this Ticket ticket)
        {
            return selectUtility.GetCategoryList(false, ticket.Category);
        }

    }
}
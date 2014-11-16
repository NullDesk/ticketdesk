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
using System.Diagnostics;
using System.Reflection;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    public static class TicketTextUtility
    {
        //TODO: Move these static strings to configuration so values can be modified by the admin
        public static string Create = "created the ticket";
        public static string CreateOnBehalfOf = "created the ticket on behalf of {0}";
        public static string AddComment = "added comment";
        public static string TakeOver = "has taken over the ticket{0}";
        public static string TakeOverWithPriority = "has taken over the ticket{0} with a priority of {1}";
        public static string Resolve = "resolved the ticket";
        public static string Close = "closed the ticket";
        public static string ForceClose = "closed the ticket by force";
        public static string ReOpen = "re-opened the ticket{0}{1}";
        public static string GiveUp = "has given up on the ticket";
        public static string Assign = "assigned the ticket to {1}";
        public static string AssignWithPriority = "assigned the ticket to {1} at a priority of {2}";
        public static string ReAssign = "reassigned the ticket from {0} to {1}";
        public static string ReAssignWithPriority = "reassigned the ticket from {0} to {1} at a priority of {2}";
        public static string Pass = "passed the ticket to {1}";
        public static string PassWithPriority = "passed the ticket to {1} at a priority of {2}";
        public static string RequestMoreInfo = "has requested more information";
        public static string SupplyMoreInfo = "has provided more information {0}";
        public static string CancelMoreInfo = "has cancelled the request for more information";
        public static string ModifyAttachments = "modified ticket attachments";
        public static string EditTicketInfo = "modified ticket";


        /// <summary>
        /// Gets the comment event text.
        /// </summary>
        /// <param name="ticketEvent">The ticket event of which to fetch the comment event.</param>
        /// <param name="commentFlag">The comment flag.</param>
        /// <param name="replacements">Any replacements values that should be passed to the string.</param>
        /// <returns>System.String.</returns>
        public static string GetCommentText(TicketActivity ticketEvent, TicketCommentFlag commentFlag, params object[] replacements)
        {
            var n = Enum.GetName(typeof(TicketActivity), ticketEvent);

            var t = typeof(TicketTextUtility);
            Debug.Assert(n != null, "n != null");
            var field = t.GetField(n, BindingFlags.Public | BindingFlags.Static);
            Debug.Assert(field != null, "field != null");
            var val = (string)field.GetValue(null);
            var appendCommentText = (commentFlag == TicketCommentFlag.CommentNotSupplied) ? " without comment" : string.Empty;

            if (replacements.Length > 0)
            {
                val = string.Format(val, replacements);
            }
            return val + appendCommentText;
        }

    }
}

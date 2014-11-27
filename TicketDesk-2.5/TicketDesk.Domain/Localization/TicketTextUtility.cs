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
using TicketDesk.Domain.Localization;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain.Localization
{
    public static class TicketTextUtility
    {


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
            var val = TicketDeskDomainText.ResourceManager.GetString("TicketActivity" + n);
            if (string.IsNullOrEmpty(val))
            {
                throw new NullReferenceException();
            }
            var appendCommentText = (commentFlag == TicketCommentFlag.CommentNotSupplied) ? TicketDeskDomainText.TicketActivityWithoutComment : string.Empty;
            if (replacements.Length > 0)
            {
                val = string.Format(val, replacements);
            }
            return val + appendCommentText;
        }
    }
}

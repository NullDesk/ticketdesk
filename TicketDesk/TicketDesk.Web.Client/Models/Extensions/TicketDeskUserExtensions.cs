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

using System.Collections.Generic;
using System.Web.Mvc;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models
{
    public static class TicketDeskUserExtensions
    {
        public static SelectList ToUserSelectList(this IEnumerable<TicketDeskUser> users, bool includeEmpty,
            string selectedUserId)
        {
            return users.ToSelectList(u => u.Id, u => u.DisplayName, selectedUserId, includeEmpty);
        }

        public static SelectList ToUserSelectList(this IEnumerable<TicketDeskUser> users, string selectedUserId, string emptyText)
        {
            return users.ToSelectList(u => u.Id, u => u.DisplayName, emptyText, null, selectedUserId);
        }
    }
}
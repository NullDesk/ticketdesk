using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Models.Extensions
{
    public static class TicketDeskUserExtensions
    {
        public static SelectList ToSelectList(this IEnumerable<TicketDeskUser> users, bool includeEmpty,
            string selectedUserId)
        {
            return users.ToSelectList(u => u.Id, u => u.DisplayName, selectedUserId, includeEmpty);
        }

        public static SelectList ToSelectList(this IEnumerable<TicketDeskUser> users, string selectedUserId, string emptyText)
        {
            return users.ToSelectList(u => u.Id, u => u.DisplayName, emptyText, null, selectedUserId);
        }
    }
}
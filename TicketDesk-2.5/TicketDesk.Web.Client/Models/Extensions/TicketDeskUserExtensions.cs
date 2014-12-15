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
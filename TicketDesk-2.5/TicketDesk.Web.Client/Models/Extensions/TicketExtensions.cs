using System.Web;
using System.Web.Mvc;
using MarkdownSharp;
using StackExchange.DataExplorer.Helpers;
using TicketDesk.Web.Client;

namespace TicketDesk.Domain.Model
{
    public static class TicketExtensions
    {
        public static UserDisplayInfo GetAssignedToInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.AssignedTo);
        }
        public static UserDisplayInfo GetCreatedByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.CreatedBy);
        }
        public static UserDisplayInfo GetOwnerInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.Owner);
        }
        public static UserDisplayInfo GetLastUpdatedByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.LastUpdateBy);
        }
        public static UserDisplayInfo GetCurrentStatusSetByInfo(this Ticket ticket)
        {
            return GetUserInfo(ticket.CurrentStatusSetBy);
        }

        private static UserDisplayInfo GetUserInfo(string userId)
        {
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.InfoCache.GetUserInfo(userId) ?? new UserDisplayInfo();
        }

        public static HtmlString HtmlDetails(this Ticket ticket)
        {
            var content = (ticket.IsHtml) ? ticket.Details : ticket.Details.HtmlFromMarkdown();
            return new HtmlString(HtmlUtilities.Safe(content));
        }




    }
}
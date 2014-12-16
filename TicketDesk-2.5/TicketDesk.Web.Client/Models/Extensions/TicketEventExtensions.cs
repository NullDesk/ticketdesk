using System.Web;
using StackExchange.DataExplorer.Helpers;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public static class TicketEventExtensions
    {
        public static UserDisplayInfo GetEventByInfo(this TicketEvent ticketEvent)
        {
            return UserDisplayInfo.GetUserInfo(ticketEvent.EventBy);
        }

        public static HtmlString HtmlComment(this TicketEvent ticketEvent)
        {
            var content = (ticketEvent.IsHtml) ? ticketEvent.Comment : ticketEvent.Comment.HtmlFromMarkdown();
            return new HtmlString(HtmlUtilities.Safe(content));
        }
    }
}
// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Web;
using StackExchange.DataExplorer.Helpers;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client;

namespace TicketDesk.Domain.Model
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
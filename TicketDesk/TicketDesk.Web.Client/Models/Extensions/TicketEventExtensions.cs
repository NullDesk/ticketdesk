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

using System.Web;
using System.Web.Mvc;
using Ganss.XSS;
using TicketDesk.Web.Client;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventExtensions
    {
        public static UserDisplayInfo GetEventByInfo(this TicketEvent ticketEvent)
        {
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.GetUserInfo(ticketEvent.EventBy);
        }

        public static HtmlString HtmlComment(this TicketEvent ticketEvent)
        {
            var content = (ticketEvent.IsHtml) ? ticketEvent.Comment : ticketEvent.Comment.HtmlFromMarkdown();
            var san = new HtmlSanitizer();

            return new HtmlString(san.Sanitize(content));
            
        }
    }
}
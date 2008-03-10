// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Web;

namespace TicketDesk.Engine.Linq
{
    public partial class TicketDataDataContext
    {

        partial void UpdateTicket(Ticket instance)
        {
            instance.LastUpdateBy = HttpContext.Current.User.Identity.GetFormattedUserName();
            instance.LastUpdateDate = DateTime.Now;
            ExecuteDynamicUpdate(instance);
        }
        

        partial void InsertTicket(Ticket instance)
        {
            instance.LastUpdateBy = HttpContext.Current.User.Identity.GetFormattedUserName();
            instance.LastUpdateDate = DateTime.Now;
            ExecuteDynamicInsert(instance);
        }
        
        partial void UpdateTicketComment(TicketComment instance)
        {
            string user = HttpContext.Current.User.Identity.GetFormattedUserName();
            DateTime dt = DateTime.Now;
            instance.CommentedBy = user;
            instance.CommentedDate = dt;
            instance.Ticket.LastUpdateBy = user;
            instance.Ticket.LastUpdateDate = dt;
            ExecuteDynamicUpdate(instance);
        }

        partial void InsertTicketComment(TicketComment instance)
        {
            string user = HttpContext.Current.User.Identity.GetFormattedUserName();
            DateTime dt = DateTime.Now;
            instance.CommentedBy = user;
            instance.CommentedDate = dt;
            instance.Ticket.LastUpdateBy = user;
            instance.Ticket.LastUpdateDate = dt;
            ExecuteDynamicInsert(instance);
        }

        partial void InsertTicketAttachment(TicketAttachment instance)
        {
            string user = HttpContext.Current.User.Identity.GetFormattedUserName();
            DateTime dt = DateTime.Now;
            instance.UploadedBy = user;
            instance.UploadedDate = dt;
            instance.Ticket.LastUpdateBy = user;
            instance.Ticket.LastUpdateDate = dt;
            ExecuteDynamicInsert(instance);
        }

        partial void UpdateTicketAttachment(TicketAttachment instance)
        {
            string user = HttpContext.Current.User.Identity.GetFormattedUserName();
            DateTime dt = DateTime.Now;
            instance.UploadedBy = user;
            instance.UploadedDate = dt;
            instance.Ticket.LastUpdateBy = user;
            instance.Ticket.LastUpdateDate = dt;
            ExecuteDynamicUpdate(instance);
        }
    }
}

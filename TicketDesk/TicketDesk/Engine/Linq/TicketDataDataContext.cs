using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine;

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

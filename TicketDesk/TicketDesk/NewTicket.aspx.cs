using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.DirectoryServices;
using TicketDesk.Engine.Linq;

namespace TicketDesk
{
    public partial class NewTicket : System.Web.UI.Page
    {
        protected void CreateTicketButton_Click(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                TicketDataDataContext context = new TicketDataDataContext();
                Ticket ticket = NewTicketForm.GetNewTicket();
                if(ticket != null)
                {
                    //if there is an existing ticket in DB with "New" status and the same title don't submit... this is probably a duplicate submission.
                    if(context.Tickets.Count(t => (t.Title == ticket.Title && t.CurrentStatus == "New")) < 1)
                    {
                        context.Tickets.InsertOnSubmit(ticket);
                        context.SubmitChanges();
                        Page.Response.Redirect(string.Format("ViewTicket.aspx?id={0}", ticket.TicketId), true);
                    }
                    else
                    {
                        MessageLabel.Text = "Failed to create ticket. Another ticket was recently created with the same title.";
                    }
                }
                MessageLabel.Text = "Unable to create ticket.";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MessageLabel.Text = string.Empty;
        }
    }
}

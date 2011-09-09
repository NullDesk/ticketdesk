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
using System.Linq;
using System.Web.UI;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;

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
                        NotificationService.QueueTicketEventNotification(ticket.TicketComments[0]);
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

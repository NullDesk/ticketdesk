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
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
namespace TicketDesk
{
    public delegate void TicketPropertyChangedDelegate();
    public delegate void TicketAttachmentRemovedDelegate(int fileId);
    public partial class ViewTicket : System.Web.UI.Page
    {
        private TicketDataDataContext ctx = new TicketDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

            int id = -1;
            try
            {
                id = Convert.ToInt32(Page.Request.QueryString["id"] ?? "-1");
            }
            catch
            {
            }
            if(id != -1)
            {
                DisplayTicketView.TicketToDisplay = ctx.Tickets.Single(t => t.TicketId == id);
                DisplayTicketView.EnableEditControls = (DisplayTicketView.TicketToDisplay.CurrentStatus != "Closed");
                DisplayTicketView.TicketChanged += new TicketPropertyChangedDelegate(TicketChanged);
                DisplayTicketView.TicketAttachmentRemoved += new TicketAttachmentRemovedDelegate(TicketAttachmentRemoved);
            }
            else
            {
                Page.Response.Redirect("~/Default.aspx");
            }
        }

        void TicketAttachmentRemoved(int fileId)
        {
            TicketAttachment attachment = DisplayTicketView.TicketToDisplay.TicketAttachments.Single(a => a.FileId == fileId);
            ctx.TicketAttachments.DeleteOnSubmit(attachment);
            TicketChanged();
        }

        void TicketChanged()
        {
            DisplayTicketView.EnableEditControls = (DisplayTicketView.TicketToDisplay.CurrentStatus != "Closed");
            ctx.SubmitChanges();
            NotificationManager.SendTicketChangedNotification(DisplayTicketView.TicketToDisplay, Page);
        }
    }
}

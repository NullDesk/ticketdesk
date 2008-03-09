using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;
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
            ctx.SubmitChanges();
            NotificationManager.SendTicketChangedNotification(DisplayTicketView.TicketToDisplay, Page);
        }
    }
}

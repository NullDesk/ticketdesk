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
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class ReOpenPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowReOpenButton.Visible = (TicketToDisplay.CurrentStatus == "Closed") || (TicketToDisplay.CurrentStatus == "Resolved");

        }
        public event TicketPropertyChangedDelegate ReOpened;
        private Ticket _ticket;
        public Ticket TicketToDisplay
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ReOpenButton_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string oldStatus = TicketToDisplay.CurrentStatus;
            if(TicketToDisplay.Owner != Page.User.Identity.GetFormattedUserName() && (!SecurityManager.IsStaffOrAdmin))
            {
                TicketToDisplay.Owner = Page.User.Identity.GetFormattedUserName();
            }

            TicketToDisplay.CurrentStatus = "Active";
            TicketToDisplay.CurrentStatusDate = now;
            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();

            if(oldStatus == "Closed")
            {
                TicketToDisplay.AssignedTo = null;
            }
            TicketComment comment = new TicketComment();
            
            comment.CommentEvent = string.Format("has re-opened the ticket");
           
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = CommentsTextBox.Text.Trim();
            }
            
            TicketToDisplay.TicketComments.Add(comment);

            ReOpenModalPopupExtender.Hide();
            if(ReOpened != null)
            {
                ReOpened();
            }
            
        }
    }
}
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
    public partial class TakeOverPopup : System.Web.UI.UserControl
    {
        public TakeOverPopup()
        {
            PreRender += new EventHandler(TakeOverPopup_PreRender);
        }

        void TakeOverPopup_PreRender(object sender, EventArgs e)
        {
            bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());
            ShowTakeOverButton.Visible =
            (
                SecurityManager.IsStaffOrAdmin &&
                !isAssignedToMe &&
                TicketToDisplay.CurrentStatus != "Closed"
            );
            SetPriorityPanel.Visible = string.IsNullOrEmpty(TicketToDisplay.Priority);
        }
        public event TicketPropertyChangedDelegate TakenOver;
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

        protected void TakeOverButton_Click(object sender, EventArgs e)
        {
            string oldAssigned = TicketToDisplay.AssignedTo;
            TicketToDisplay.AssignedTo = Page.User.Identity.GetFormattedUserName();

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;
            bool setPriority = SetPriorityPanel.Visible && PriorityList.SelectedIndex > -1;
            if(setPriority)
            {
                TicketToDisplay.Priority = PriorityList.SelectedValue;
            }

            if(!string.IsNullOrEmpty(oldAssigned))
            {
                comment.CommentEvent = string.Format("has taken over the ticket from {0}", SecurityManager.GetUserDisplayName(oldAssigned));
            }
            else
            {
                comment.CommentEvent = "has taken over the ticket";
            }

            if(setPriority)
            {
                comment.CommentEvent = string.Format("{0} at a priority of {1}", comment.CommentEvent, TicketToDisplay.Priority);
            }
            
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = CommentsTextBox.Text.Trim();
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }
            TicketToDisplay.TicketComments.Add(comment);

            TakeOverModalPopupExtender.Hide();
            if(TakenOver != null)
            {
                TakenOver();
            }

        }
    }
}
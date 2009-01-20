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
using System.Web.UI;
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


            if(!Page.IsPostBack)
            {
                PriorityList.DataSource = SettingsManager.PrioritiesList;
                PriorityList.DataBind();
            }

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
            
            comment.IsHtml = true;
            if(CommentsTextBox.Value != string.Empty)
            {
                comment.Comment = CommentsTextBox.Value;
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }
            TicketToDisplay.TicketComments.Add(comment);

            TakeOverModalPopupExtender.Hide();
            if(TakenOver != null)
            {
                TakenOver(comment);
            }

        }
    }
}
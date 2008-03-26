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
                comment.Comment = Server.HtmlEncode(CommentsTextBox.Text).Trim();
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
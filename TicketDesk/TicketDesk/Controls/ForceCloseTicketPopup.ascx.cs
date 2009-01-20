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
    public partial class ForceCloseTicketPopup : System.Web.UI.UserControl
    {


        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());
            bool isOwnerMe = (TicketToDisplay.Owner == Page.User.Identity.GetFormattedUserName());
            bool isClosed = TicketToDisplay.CurrentStatus == "Closed";
            bool isResolved = (TicketToDisplay.CurrentStatus == "Resolved");
            ShowForceCloseTicketButton.Visible =
                (
                    !isClosed
                )
                &&
                (
                        (isOwnerMe && !isResolved) ||
                        (isAssignedToMe && !isClosed && !isOwnerMe)
                );
        }
        public event TicketPropertyChangedDelegate TicketForceClosed;
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

            lblCommentRequired.Visible = false;
        }

        protected void ForceCloseTicketButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CommentsTextBox.Value))
            {
                DateTime now = DateTime.Now;

                TicketToDisplay.CurrentStatus = "Closed";
                TicketToDisplay.CurrentStatusDate = now;
                TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();


                TicketComment comment = new TicketComment();
                comment.CommentEvent = string.Format("has closed the ticket by force");
                if (CommentsTextBox.Value != string.Empty)
                {
                    comment.Comment = CommentsTextBox.Value;
                }




                comment.IsHtml = true;

                TicketToDisplay.TicketComments.Add(comment);

                ForceCloseTicketModalPopupExtender.Hide();
                if (TicketForceClosed != null)
                {
                    TicketForceClosed(comment);
                }
            }
            else
            {
                ForceCloseTicketModalPopupExtender.Show();
                lblCommentRequired.Visible = true;
            }
        }
    }
}
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
    public partial class CancelMoreInfoPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());
            bool isOwnerMe = (TicketToDisplay.Owner == Page.User.Identity.GetFormattedUserName());
            ShowCancelMoreInfoButton.Visible = isAssignedToMe && (TicketToDisplay.CurrentStatus == "More Info");

        }

        public event TicketPropertyChangedDelegate MoreInfoCanceled;
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

        protected void CancelMoreInfoButton_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            TicketToDisplay.CurrentStatus = "Active";
            TicketToDisplay.CurrentStatusDate = now;
            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();


            TicketComment comment = new TicketComment();
            comment.CommentEvent = string.Format("has cancelled the request for more information");
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = Server.HtmlEncode(CommentsTextBox.Text).Trim();
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }



            
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = Server.HtmlEncode(CommentsTextBox.Text).Trim();
            }

            TicketToDisplay.TicketComments.Add(comment);

            CancelMoreInfoModalPopupExtender.Hide();
            if(MoreInfoCanceled != null)
            {
                MoreInfoCanceled();
            }

        }
    }
}
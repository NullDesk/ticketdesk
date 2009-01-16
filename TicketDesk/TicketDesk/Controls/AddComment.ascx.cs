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
    public partial class AddComment : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());

            ResolveCommentCheckBox.Visible = (TicketToDisplay.CurrentStatus == "Active") && isAssignedToMe;
            ProvideInfoCommentCheckBox.Visible = (TicketToDisplay.CurrentStatus == "More Info");
            AddCommentsTextBox.Enabled = (TicketToDisplay.CurrentStatus != "Closed");
            AddCommentButton.Enabled = (TicketToDisplay.CurrentStatus != "Closed");
        }
        public event TicketPropertyChangedDelegate CommentChanged;
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

        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            if (ResolveCommentCheckBox.Visible && ResolveCommentCheckBox.Checked)
            {
                TicketToDisplay.CurrentStatus = "Resolved";
                TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                TicketToDisplay.CurrentStatusDate = now;
                comment.CommentEvent = "resolved the ticket";
            }
            else if (ResolveCommentCheckBox.Visible && ProvideInfoCommentCheckBox.Checked)
            {
                TicketToDisplay.CurrentStatus = "Active";
                TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                TicketToDisplay.CurrentStatusDate = now;
                comment.CommentEvent = "has provided more information";
            }
            else
            {
                comment.CommentEvent = "added comment";
            }
            
            comment.IsHtml = false;
            comment.Comment = Server.HtmlEncode(AddCommentsTextBox.Text).Trim();
            AddCommentsTextBox.Text = string.Empty;
            TicketToDisplay.TicketComments.Add(comment);

            
            //NewCommentPanel.Height = Unit.Parse("0px");
            if(CommentChanged != null)
            {
                CommentChanged(comment);
            }
        }

    }
}
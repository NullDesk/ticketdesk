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
    public partial class GiveUpPopup : System.Web.UI.UserControl
    {
        

        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());
            ShowGiveUpButton.Visible = SecurityManager.IsStaffOrAdmin && isAssignedToMe && (TicketToDisplay.CurrentStatus != "Closed");
       
        }
        public event TicketPropertyChangedDelegate GivenUp;
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

        protected void GiveUpButton_Click(object sender, EventArgs e)
        {
            TicketToDisplay.AssignedTo = null;

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

           
            comment.CommentEvent = "has given up on the ticket";
           
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = Server.HtmlEncode(CommentsTextBox.Text).Trim();
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }
            TicketToDisplay.TicketComments.Add(comment);

            GiveUpModalPopupExtender.Hide();
            if(GivenUp!= null)
            {
                GivenUp(comment);
            }
            
        }
    }
}
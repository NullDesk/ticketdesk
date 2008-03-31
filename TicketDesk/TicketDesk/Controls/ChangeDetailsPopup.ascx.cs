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
    public partial class ChangeDetailsPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangeDetailsButton.Visible = TicketToDisplay.CurrentStatus != "Closed";
        }

        public event TicketPropertyChangedDelegate DetailsChanged;
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
            if(!Page.IsPostBack && Visible)
            {
                if(TicketToDisplay.IsHtml)
                {
                    EditDetailsControl.Details = TicketToDisplay.Details;
                }
                else
                {
                    EditDetailsControl.Details = Server.HtmlDecode(TicketToDisplay.Details);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangePriorityButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangeDetailsButton_Click(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                TicketToDisplay.Details = Server.HtmlEncode(EditDetailsControl.Details);
                TicketToDisplay.IsHtml = false;

                TicketComment comment = new TicketComment();
                DateTime now = DateTime.Now;


                comment.CommentEvent = "edited the details for the ticket";

               
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

                ChangeDetailsModalPopupExtender.Hide();
                if(DetailsChanged != null)
                {
                    DetailsChanged();
                }
            }
        }
    }
}
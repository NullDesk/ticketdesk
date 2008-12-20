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
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    
    public partial class ChangeAffectsCustomerPopup : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangeAffectsCustomerButton.Visible = TicketToDisplay.CurrentStatus != "Closed";
        }

        public event TicketPropertyChangedDelegate AffectsCustomerChanged;
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
                if(TicketToDisplay.AffectsCustomer)
                {
                    AffectsCustomerList.Items.FindByValue("yes").Selected = true;
                }
                else
                {
                    AffectsCustomerList.Items.FindByValue("no").Selected = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangeAffectsCustomerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangeAffectsCustomerButton_Click(object sender, EventArgs e)
        {
            TicketToDisplay.AffectsCustomer = (AffectsCustomerList.SelectedValue == "yes");

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;
            string dText = "does not";
            if(TicketToDisplay.AffectsCustomer)
            {
                dText = "does";
            }
            comment.CommentEvent = string.Format("indicates that the ticket {0} affect customers", dText);
           
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = Server.HtmlEncode(CommentsTextBox.Text).Trim();
            }
            
            TicketToDisplay.TicketComments.Add(comment);

            ChangeAffectsCustomerModalPopupExtender.Hide();
            if(AffectsCustomerChanged != null)
            {
                AffectsCustomerChanged(comment);
            }
        }
    }
}
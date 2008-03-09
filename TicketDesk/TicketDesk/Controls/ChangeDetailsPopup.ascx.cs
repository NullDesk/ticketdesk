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
                    EditDetailsControl.Details = TicketToDisplay.Details.FormatAsHtml();
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
                TicketToDisplay.Details = EditDetailsControl.Details;
                TicketToDisplay.IsHtml = false;

                TicketComment comment = new TicketComment();
                DateTime now = DateTime.Now;


                comment.CommentEvent = "edited the details for the ticket";

               
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

                ChangeDetailsModalPopupExtender.Hide();
                if(DetailsChanged != null)
                {
                    DetailsChanged();
                }
            }
        }
    }
}
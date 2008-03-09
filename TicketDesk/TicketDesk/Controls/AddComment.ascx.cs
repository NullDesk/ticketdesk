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
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;

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

            if(ResolveCommentCheckBox.Checked)
            {
                TicketToDisplay.CurrentStatus = "Resolved";
                TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                TicketToDisplay.CurrentStatusDate = now;
                comment.CommentEvent = "resolved the ticket";
            }
            else if(ProvideInfoCommentCheckBox.Checked)
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
            comment.Comment = AddCommentsTextBox.Text.Trim();
            AddCommentsTextBox.Text = string.Empty;
            TicketToDisplay.TicketComments.Add(comment);

            
            //NewCommentPanel.Height = Unit.Parse("0px");
            if(CommentChanged != null)
            {
                CommentChanged();
            }
        }

    }
}
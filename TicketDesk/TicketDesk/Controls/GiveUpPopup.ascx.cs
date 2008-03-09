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
                comment.Comment = CommentsTextBox.Text.Trim();
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment";
            }
            TicketToDisplay.TicketComments.Add(comment);

            GiveUpModalPopupExtender.Hide();
            if(GivenUp!= null)
            {
                GivenUp();
            }
            
        }
    }
}
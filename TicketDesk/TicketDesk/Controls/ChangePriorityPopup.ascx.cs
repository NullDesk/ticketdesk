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
    
    public partial class ChangePriorityPopup : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangePriorityButton.Visible = TicketToDisplay.CurrentStatus != "Closed" && (SecurityManager.IsStaffOrAdmin || SecurityManager.SubmitterCanEditPriority);

        }

       

        public event TicketPropertyChangedDelegate PriorityChanged;
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
                BuildPriorityList();
                
            }
           
        }

        private void BuildPriorityList()
        {
            PriorityList.SelectedIndex = -1;
            PriorityList.Items.Clear();
            if(TicketToDisplay.Priority != "Low")
            {
                PriorityList.Items.Add("Low");
            }
            if(TicketToDisplay.Priority != "Medium")
            {
                PriorityList.Items.Add("Medium");
            }
            if(TicketToDisplay.Priority != "High")
            {
                PriorityList.Items.Add("High");
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangePriorityButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangePriorityButton_Click(object sender, EventArgs e)
        {
            string oldPriority = TicketToDisplay.Priority;
            TicketToDisplay.Priority = PriorityList.SelectedValue;
            

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            if(!string.IsNullOrEmpty(oldPriority))
            {
                comment.CommentEvent = string.Format("changed the priority from {0} to {1}", oldPriority, TicketToDisplay.Priority);
            }
            else
            {
                comment.CommentEvent = string.Format("set the priority to {0}", TicketToDisplay.Priority);
            }
           
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

            ChangePriorityModalPopupExtender.Hide();
            if(PriorityChanged != null)
            {
                PriorityChanged();
            }
            BuildPriorityList();
            
        }
    }
}
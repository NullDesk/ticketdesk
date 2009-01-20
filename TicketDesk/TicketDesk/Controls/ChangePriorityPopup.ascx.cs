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

            string[] priorities = SettingsManager.PrioritiesList;
            foreach(string p in priorities)
            {
                if(TicketToDisplay.Priority != p)
                {
                    PriorityList.Items.Add(p);
                }
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
           
            comment.IsHtml = true;
            if(CommentsTextBox.Value != string.Empty)
            {
                comment.Comment = CommentsTextBox.Value;
            }
            else
            {
                comment.CommentEvent = comment.CommentEvent + " without comment"; 
            }
            TicketToDisplay.TicketComments.Add(comment);

            ChangePriorityModalPopupExtender.Hide();
            if(PriorityChanged != null)
            {
                PriorityChanged(comment);
            }
            BuildPriorityList();
            
        }
    }
}
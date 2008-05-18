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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class AssignPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(TicketToDisplay.AssignedTo))
            {
                ShowAssignButton.Text = "Assign";
            }
            else
            {
                ShowAssignButton.Text = "Re-Assign";
            }
            ShowAssignButton.Visible = (SecurityManager.IsStaffOrAdmin && TicketToDisplay.CurrentStatus != "Closed");

            SetPriorityPanel.Visible = string.IsNullOrEmpty(TicketToDisplay.Priority);
        }
        public event TicketPropertyChangedDelegate AssignedToChanged;
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

            if(!Page.IsPostBack)
            {
                PriorityList.DataSource = SettingsManager.PrioritiesList;
                PriorityList.DataBind();
            }
        }

        private void BuildUserList()
        {
            AssignDropDownList.SelectedIndex = -1;
            AssignDropDownList.Items.Clear();
            AssignDropDownList.Items.Add(new ListItem("-- select --", "-"));
            AssignDropDownList.Items.AddRange(GetUserList());
        }


        public ListItem[] GetUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetHelpDeskUsers();
            foreach(User user in users)
            {
                if(user.Name.ToUpperInvariant() != HttpContext.Current.User.Identity.Name.ToUpperInvariant())
                {
                    if(TicketToDisplay.AssignedTo == null || user.Name.ToUpperInvariant() != TicketToDisplay.AssignedTo.ToUpperInvariant())
                    {
                        returnUsers.Add(new ListItem(user.DisplayName,user.Name));
                    }
                }
            }
            return returnUsers.ToArray();
        }


        protected void AssignButton_Click(object sender, EventArgs e)
        {
            string oldAssigned = TicketToDisplay.AssignedTo;
            TicketToDisplay.AssignedTo = AssignDropDownList.SelectedValue;

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            bool setPriority = SetPriorityPanel.Visible && PriorityList.SelectedIndex > -1;
            if(setPriority)
            {
                TicketToDisplay.Priority = PriorityList.SelectedValue;
            }

            if(!string.IsNullOrEmpty(oldAssigned) && oldAssigned.ToUpperInvariant() != Page.User.Identity.GetFormattedUserName().ToUpperInvariant())
            {
                comment.CommentEvent = string.Format("reassigned the ticket from {0} to {1}", SecurityManager.GetUserDisplayName(oldAssigned), SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));
            }
            else if(!string.IsNullOrEmpty(oldAssigned))
            {
                comment.CommentEvent = string.Format("passed the ticket to {0}", SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));

            }
            else
            {
                comment.CommentEvent = string.Format("assigned the ticket to {0}", SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo));
            }

            if(setPriority)
            {
                comment.CommentEvent = string.Format("{0} at a priority of {1}", comment.CommentEvent, TicketToDisplay.Priority);
            }
            

           
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

            AssignModalPopupExtender.Hide();
            if(AssignedToChanged != null)
            {
                AssignedToChanged();
            }

        }

        protected void ShowAssignButton_Click(object sender, EventArgs e)
        {
            BuildUserList();
            AssignModalPopupExtender.Show();
            AssignDropDownList.Focus();
        }
    }
}
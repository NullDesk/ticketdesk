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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;

namespace TicketDesk.TicketViewer
{
    public partial class TicketActivityEditor : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate TicketActivityCompleted;

        public event EventHandler TicketActivityCanceled;
        public event EventHandler TicketActivityFailed;

        private void ActivityComplete(TicketComment comment)
        {
            if (TicketActivityCompleted != null)
            {
                TicketActivityCompleted(comment);
            }
        }

        private void ActivityCanceled()
        {
            if (TicketActivityCanceled != null)
            {
                TicketActivityCanceled(this, EventArgs.Empty);
            }
        }

        private bool activityFailed = false;
        private void ActivityFailed()
        {
            activityFailed = true;
            if (TicketActivityFailed != null)
            {
                TicketActivityFailed(this, EventArgs.Empty);
            }
        }

        public String Activity
        {
            get { return this.ViewState["activity"] as string; }
            set { this.ViewState.Add("activity", value); }
        }

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!activityFailed)
            {
                SetupForActivity();
            }
        }

        private void SetupForActivity()
        {

            CommentPanel.Visible = true;
            CommitButton.Visible = true;
            AssignPanel.Visible = false;
            NoChangesPanel.Visible = false;

            RequiredCommentLabel.Visible = false;
            SupplyMoreInfoPanel.Visible = false;
            AddCommentPanel.Visible = false;
            ReopenStaffPanel.Visible = false;
            ActivityNotAllowedPanel.Visible = false;
            CancelButton.Text = "Nevermind";
            if (!CheckIsActivityAllowed())
            {
                Activity = "Invalid";
            }

            switch (Activity)
            {
                case "NoChanges":
                    NoChangesPanel.Visible = true;
                    CommentPanel.Visible = false;
                    CommitButton.Visible = false;
                    CancelButton.Text = "Continue";
                    ActivityFailed();
                    break;
                case "Invalid":
                    ActivityNotAllowedPanel.Visible = true;
                    CommentPanel.Visible = false;
                    CommitButton.Visible = false;
                    CancelButton.Text = "Refresh";
                    ActivityFailed();
                    break;
                case "EditTicket":
                    ActivityLabel.Text = "Edit Ticket";
                    CommitButton.Text = "Save Changes";
                    CommentFieldLabel.Text = "Notes & Comments:";
                    ActivityDescription.Text = "Make changes to a ticket's details, title, or other fields.";
                    break;
                case "AddComment":
                    ActivityLabel.Text = "Add a Comment";
                    CommitButton.Text = "Add Comment";
                    CommentFieldLabel.Text = "Comments:";
                    ActivityDescription.Text = "Add a comment to the ticket.";
                    AddCommentPanel.Visible = true;
                    ResolveCheckBox.Checked = false;
                    break;
                case "SupplyInfo":
                    ActivityLabel.Text = "Provide Additional Information";
                    CommitButton.Text = "Provide Info";
                    CommentFieldLabel.Text = "Additional Information & Comments:";
                    SupplyMoreInfoPanel.Visible = true;
                    SupplyInfoActivateTicketCheckBox.Checked = true;
                    ActivityDescription.Text = "Supply additional information as requested by the TicketDesk staff and (optionally) mark the ticket active again.";
                    break;
                case "Resolve":
                    ActivityLabel.Text = "Resolve Ticket";
                    CommitButton.Text = "Resolve";
                    CommentFieldLabel.Text = "Resolution:";
                    ActivityDescription.Text = "Mark the ticket resolved.";
                    break;
                case "RequestMoreInfo":
                    ActivityLabel.Text = "Request More Information";
                    CommitButton.Text = "Request Info";
                    CommentFieldLabel.Text = "Describe the information required:";
                    ActivityDescription.Text = "Request more information from the ticket's owner.";
                    break;
                case "CancelMoreInfo":
                    ActivityLabel.Text = "Cancel Request for More Information";
                    CommitButton.Text = "Cancel Request";
                    CommentFieldLabel.Text = "Comments:";
                    ActivityDescription.Text = "Cancel the request for more information and mark the ticket active again.";
                    break;
                case "CloseTicket":
                    ActivityLabel.Text = "Close Ticket";
                    CommitButton.Text = "Close Ticket";
                    CommentFieldLabel.Text = "Comments:";
                    ActivityDescription.Text = "Close the ticket.";
                    break;
                case "ReopenTicket":
                    ActivityLabel.Text = "Re-Open Ticket";
                    CommitButton.Text = "Re-Open";
                    CommentFieldLabel.Text = "Reason for reactivation:";
                    ActivityDescription.Text = "Mark ticket as active again.";
                    if (SecurityManager.IsStaff)
                    {
                        ReopenStaffPanel.Visible = true;
                        ReopenAssignToMe.Checked = true;
                        ReopenOwnedByMe.Checked = false;
                    }
                    break;
                case "TakeOver":
                    ActivityLabel.Text = "Take Over Ticket";
                    CommitButton.Text = "Take Over";
                    CommentFieldLabel.Text = "Comments";
                    ActivityDescription.Text = "Assign the ticket to yourself.";
                    break;
                case "Assign":
                    if (string.IsNullOrEmpty(TicketToDisplay.AssignedTo))
                    {
                        ActivityLabel.Text = "Reassign Ticket";
                        CommitButton.Text = "Reassign";
                    }
                    else
                    {
                        ActivityLabel.Text = "Assign Ticket";
                        CommitButton.Text = "Assign";
                    }
                    CommentFieldLabel.Text = "Comments:";
                    ActivityDescription.Text = "Assign the ticket to another member of the TicketDesk staff.";
                    AssignPanel.Visible = true;

                    BuildPriorityList();
                    BuildAssignedUserList();

                    break;
                case "GiveUp":
                    ActivityLabel.Text = "Give Up!";
                    CommitButton.Text = "Give Up";
                    CommentFieldLabel.Text = "Reason for giving up:";
                    ActivityDescription.Text = "Mark the ticket as unassigned so it can be taken over or re-assigned to another member of the TicketDesk staff";
                    break;
                case "ForceClose":
                    ActivityLabel.Text = "Close Ticket by Force";
                    CommitButton.Text = "Close";
                    CommentFieldLabel.Text = "Reason for force closing:";
                    ActivityDescription.Text = "Marks the ticket as closed immediatly, bypassing the normal resolved state.";
                    break;

            }
        }

        private bool CheckIsActivityAllowed()
        {

            bool isAllowed = TicketToDisplay.CheckSecurityForTicketActivity(Activity, Page.User.Identity.GetFormattedUserName());

            return isAllowed;

        }

        protected void CommitButton_Click(object sender, EventArgs e)
        {

            TicketComment comment = PerformActivity();
            if (comment != null)
            {
                ActivityComplete(comment);
                if (Activity != "NoChanges")
                {
                    Activity = null;
                    CommentText.Value = string.Empty;
                }

            }

        }

        private TicketComment PerformActivity()
        {

            string eventText = null;
            if (CheckIsActivityAllowed())
            {
                switch (Activity)
                {
                    case "NoChanges":
                        break;
                    case "EditTicket":
                        eventText = "has edited ticket fields";
                        break;
                    case "AddComment":
                        if (EnforceRequiredComment())
                        {
                            if (ResolveCheckBox.Checked)
                            {
                                eventText = ResolveTicket(eventText);
                            }
                            else
                            {
                                eventText = "added comment";
                            }
                        }
                        break;
                    case "SupplyInfo":
                        if (EnforceRequiredComment())
                        {
                            if (SupplyInfoActivateTicketCheckBox.Checked)
                            {
                                eventText = "has provided more information";

                                TicketToDisplay.CurrentStatus = "Active";
                                TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                                TicketToDisplay.CurrentStatusDate = DateTime.Now;
                            }
                            else
                            {
                                eventText = "added comment";
                            }
                        }
                        break;
                    case "Resolve":
                        if (EnforceRequiredComment())
                        {
                            eventText = ResolveTicket(eventText);
                        }
                        break;
                    case "RequestMoreInfo":
                        if (EnforceRequiredComment())
                        {
                            eventText = "has requested more information";
                            TicketToDisplay.CurrentStatus = "More Info";
                            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                            TicketToDisplay.CurrentStatusDate = DateTime.Now;
                        }
                        break;
                    case "CancelMoreInfo":
                        eventText = GetOptionalCommentEventText("has cancelled the request for more information");
                        TicketToDisplay.CurrentStatus = "Active";
                        TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                        TicketToDisplay.CurrentStatusDate = DateTime.Now;
                        break;
                    case "CloseTicket":
                        eventText = GetOptionalCommentEventText("has closed the ticket");
                        TicketToDisplay.CurrentStatus = "Closed";
                        TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                        TicketToDisplay.CurrentStatusDate = DateTime.Now;
                        break;
                    case "ReopenTicket":
                        if (EnforceRequiredComment())
                        {
                            eventText = "has re-opened the ticket";

                            if ((SecurityManager.IsStaff && Page.User.Identity.GetFormattedUserName() != TicketToDisplay.Owner && ReopenOwnedByMe.Checked) || (!SecurityManager.IsStaff && Page.User.Identity.GetFormattedUserName() != TicketToDisplay.Owner))
                            {
                                TicketToDisplay.Owner = Page.User.Identity.GetFormattedUserName();
                                eventText = eventText + " as the owner";
                            }


                            if (SecurityManager.IsStaff && ReopenAssignToMe.Checked)
                            {
                                TicketToDisplay.AssignedTo = Page.User.Identity.GetFormattedUserName();
                                eventText = eventText + " and assigned it to themself";
                            }
                            else
                            {
                                TicketToDisplay.AssignedTo = null;
                            }

                            TicketToDisplay.CurrentStatus = "Active";
                            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                            TicketToDisplay.CurrentStatusDate = DateTime.Now;
                        }
                        break;
                    case "TakeOver":

                        string oldAssigned = TicketToDisplay.AssignedTo;

                        if (!string.IsNullOrEmpty(oldAssigned))
                        {
                            eventText = GetOptionalCommentEventText(string.Format("has taken over the ticket from {0}", SecurityManager.GetUserDisplayName(oldAssigned)));
                        }
                        else
                        {
                            eventText = GetOptionalCommentEventText("has taken over the ticket");
                        }

                        TicketToDisplay.AssignedTo = Page.User.Identity.GetFormattedUserName();

                        break;
                    case "Assign":
                        string oldAssignedDisplay = SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo);
                        string newAssignedDisplay = SecurityManager.GetUserDisplayName(AssignDropDownList.SelectedValue);
                        if (string.IsNullOrEmpty(TicketToDisplay.AssignedTo))
                        {
                            eventText = string.Format("assigned the ticket to {0}", newAssignedDisplay);
                        }
                        else if (TicketToDisplay.AssignedTo == Page.User.Identity.Name)
                        {
                            eventText = string.Format("passed the ticket to {0}", newAssignedDisplay);
                        }
                        else
                        {
                            eventText = string.Format("reassigned the ticket from {0} to {1}", oldAssignedDisplay, newAssignedDisplay);
                        }

                        if (TicketToDisplay.Priority != PriorityEdit.SelectedValue)
                        {
                            TicketToDisplay.Priority = PriorityEdit.SelectedValue;
                            eventText = string.Format("{0} at a priority of {1}", eventText, TicketToDisplay.Priority);
                        }

                        eventText = GetOptionalCommentEventText(eventText);

                        TicketToDisplay.AssignedTo = AssignDropDownList.SelectedValue;
                        break;
                    case "GiveUp":
                        if (EnforceRequiredComment())
                        {
                            eventText = "has given up on the ticket";
                            TicketToDisplay.AssignedTo = null;
                        }
                        break;
                    case "ForceClose":
                        if (EnforceRequiredComment())
                        {
                            eventText = "has closed the ticket by force";
                            TicketToDisplay.CurrentStatus = "Closed";
                            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
                            TicketToDisplay.CurrentStatusDate = DateTime.Now;
                        }
                        break;
                }
            }
            TicketComment comment = null;
            if (eventText != null)
            {
                comment = new TicketComment();
                comment.IsHtml = true;
                comment.CommentEvent = eventText;
                if (!string.IsNullOrEmpty(CommentText.Value))
                {
                    comment.Comment = CommentText.Value;
                }
            }
            return comment;
        }

        private string ResolveTicket(string eventText)
        {
            eventText = "resolved the ticket";
            TicketToDisplay.CurrentStatus = "Resolved";
            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();
            TicketToDisplay.CurrentStatusDate = DateTime.Now;
            return eventText;
        }

        private string GetOptionalCommentEventText(string text)
        {
            return text + ((string.IsNullOrEmpty(CommentText.Value)) ? " without comment" : string.Empty);
        }

        private bool EnforceRequiredComment()
        {
            if (string.IsNullOrEmpty(CommentText.Value))
            {
                RequiredCommentLabel.Visible = true;
                ActivityFailed();
                return false;
            }
            return true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ActivityCanceled();
            Activity = null;
            CommentText.Value = string.Empty;
        }

        private void BuildPriorityList()
        {
            PriorityEdit.SelectedIndex = -1;
            PriorityEdit.Items.Clear();
            if (string.IsNullOrEmpty(TicketToDisplay.Priority))
            {
                PriorityEdit.Items.Add(new ListItem("-- select --", "-"));
            }
            string[] priorities = SettingsManager.PrioritiesList;
            foreach (string p in priorities)
            {
                ListItem priItem = new ListItem(p);
                priItem.Selected = (TicketToDisplay.Priority == p);
                PriorityEdit.Items.Add(priItem);
            }
        }

        private void BuildAssignedUserList()
        {
            AssignDropDownList.SelectedIndex = -1;
            AssignDropDownList.Items.Clear();
            AssignDropDownList.Items.Add(new ListItem("-- select --", "-"));
            AssignDropDownList.Items.AddRange(GetAssignedUserList());
        }


        public ListItem[] GetAssignedUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetHelpDeskUsers();
            foreach (User user in users)
            {
                if (user.Name.ToUpperInvariant() != Page.User.Identity.Name.ToUpperInvariant())
                {
                    if (TicketToDisplay.AssignedTo == null || user.Name.ToUpperInvariant() != TicketToDisplay.AssignedTo.ToUpperInvariant())
                    {
                        returnUsers.Add(new ListItem(user.DisplayName, user.Name));
                    }
                }
            }
            return returnUsers.ToArray();
        }
    }
}
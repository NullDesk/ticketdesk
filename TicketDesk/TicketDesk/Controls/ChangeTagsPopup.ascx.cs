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

    public partial class ChangeTagsPopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangeTagsButton.Visible = TicketToDisplay.CurrentStatus != "Closed" && (SecurityManager.IsStaffOrAdmin || SecurityManager.SubmitterCanEditTags);
        }

       

        public event TicketPropertyChangedDelegate TagsChanged;
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
                TagPickerControl.TagList = TicketToDisplay.TagList;
            }

        }



        /// <summary>
        /// Handles the Click event of the ChangeTagsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangeTagsButton_Click(object sender, EventArgs e)
        {
            string oldTags = TicketToDisplay.TagList;
            string[] tags = TagManager.GetTagsFromString(TagPickerControl.TagList);
            TicketToDisplay.TagList = string.Join(",", tags);

            TicketToDisplay.TicketTags.Clear();

            foreach(string tag in tags)
            {
                TicketTag tTag = new TicketTag();
                tTag.TagName = tag;
                TicketToDisplay.TicketTags.Add(tTag);
            }

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            if(!string.IsNullOrEmpty(oldTags))
            {
                comment.CommentEvent = "changed the ticket's tags";
            }
            else
            {
                comment.CommentEvent = "set the ticket's tags";
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

            ChangeTagsModalPopupExtender.Hide();
            if(TagsChanged != null)
            {
                TagsChanged();
            }


        }
    }
}
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
using TicketDesk.Engine;
using System.Web.UI.WebControls;

namespace TicketDesk.Controls
{
    
    public partial class ChangeTitleTypePopup : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
            ShowChangeTitleTypeButton.Visible = TicketToDisplay.CurrentStatus != "Closed";
        }



        public event TicketPropertyChangedDelegate TitleOrTypeChanged;
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
                TypeDropDownList.SelectedIndex = -1;
                TypeDropDownList.DataSource = SettingsManager.TicketTypesList;
                TypeDropDownList.DataBind();
                ListItem item = TypeDropDownList.Items.FindByValue(TicketToDisplay.Type);
                if(item != null)
                {
                    item.Selected = true;
                }
                TitleTextBox.Text = TicketToDisplay.Title;
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangeTitleTypeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangeTitleTypeButton_Click(object sender, EventArgs e)
        {


            string oldType = null;
            if(TypeDropDownList.SelectedValue != TicketToDisplay.Type)
            {
                oldType = TicketToDisplay.Type;
                TicketToDisplay.Type = TypeDropDownList.SelectedValue;
            }


            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            if(!string.IsNullOrEmpty(oldType))
            {
                comment.CommentEvent = string.Format("changed the type from {0} to {1}", oldType, TicketToDisplay.Type);
            }

            if(TicketToDisplay.Title != TitleTextBox.Text)
            {
                TicketToDisplay.Title = TitleTextBox.Text;
                string sep = "changed";
                if(oldType != null)//changed type too
                {
                    sep = " and";
                }
                comment.CommentEvent = string.Format("{0}{1} the title", comment.CommentEvent, sep);
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

            ChangeTitleTypeModalPopupExtender.Hide();
            if(TitleOrTypeChanged != null)
            {
                TitleOrTypeChanged(comment);
            }
            
            
        }
    }
}
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
                TypeDropDownList.Items.FindByValue(TicketToDisplay.Type).Selected = true;
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

            ChangeTitleTypeModalPopupExtender.Hide();
            if(TitleOrTypeChanged != null)
            {
                TitleOrTypeChanged();
            }
            
            
        }
    }
}
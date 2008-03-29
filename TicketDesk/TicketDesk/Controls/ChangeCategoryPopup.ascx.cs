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

namespace TicketDesk.Controls
{
    
    public partial class ChangeCategoryPopup : System.Web.UI.UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangeCategoryButton.Visible = TicketToDisplay.CurrentStatus != "Closed";
        }

        public event TicketPropertyChangedDelegate CategoryChanged;
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
                BuildCategoryList();
            }
           
        }
 
        private void BuildCategoryList()
        {
            CategoryList.SelectedIndex = -1;
            CategoryList.Items.Clear();
            foreach(string c in SettingsManager.CategoriesList)
            {
                if(TicketToDisplay.Category != c)
                {
                    CategoryList.Items.Add(c);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ChangeCategoryButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ChangeCategoryButton_Click(object sender, EventArgs e)
        {
            string oldCategory = TicketToDisplay.Category;
            TicketToDisplay.Category = CategoryList.SelectedValue;
            

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            
            comment.CommentEvent = string.Format("changed the category from {0} to {1}", oldCategory, TicketToDisplay.Category);
            
           
            
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

            ChangeCategoryModalPopupExtender.Hide();
            if(CategoryChanged != null)
            {
                CategoryChanged();
            }
            BuildCategoryList();
            
        }
    }
}
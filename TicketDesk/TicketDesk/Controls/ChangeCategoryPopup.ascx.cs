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
            if(TicketToDisplay.Category != "Hardware")
            {
                CategoryList.Items.Add("Hardware");
            }
            if(TicketToDisplay.Category != "Software")
            {
                CategoryList.Items.Add("Software");
            }
            if(TicketToDisplay.Category != "Network/Services")
            {
                CategoryList.Items.Add("Network/Services");
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
                comment.Comment = CommentsTextBox.Text.Trim();
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
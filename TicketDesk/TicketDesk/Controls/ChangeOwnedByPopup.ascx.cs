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
using System.Collections.Generic;

namespace TicketDesk.Controls
{
    
    public partial class ChangeOwnedByPopup : System.Web.UI.UserControl
    {

        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowChangeOwnedByButton.Visible = SecurityManager.IsStaffOrAdmin && TicketToDisplay.CurrentStatus != "Closed";
        }
        public event TicketPropertyChangedDelegate OwnerChanged;
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
                
            }
        }

        private void BuildUserList()
        {
            ChangeOwnedByDropDownList.SelectedIndex = -1;
            ChangeOwnedByDropDownList.Items.Clear();
            ChangeOwnedByDropDownList.Items.Add(new ListItem("-- select --", "-"));
            ChangeOwnedByDropDownList.Items.AddRange(GetUserList());
        }


        public ListItem[] GetUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetUsersInRoleType("TicketSubmittersRoleName");
            foreach(User user in users)
            {
                if(user.Name.ToUpperInvariant() != TicketToDisplay.Owner.ToUpperInvariant())
                {
                    returnUsers.Add(new ListItem(user.DisplayName, user.Name));
                }
            }
            return returnUsers.ToArray();
        }


        protected void ChangeOwnedByButton_Click(object sender, EventArgs e)
        {
            string oldOwner = TicketToDisplay.Owner;
            TicketToDisplay.Owner = ChangeOwnedByDropDownList.SelectedValue;

            TicketComment comment = new TicketComment();
            DateTime now = DateTime.Now;

            comment.CommentEvent = string.Format("changed the ticket owner from {0} to {1}", SecurityManager.GetUserDisplayName(oldOwner), SecurityManager.GetUserDisplayName(TicketToDisplay.Owner));
            
           
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

            ChangeOwnedByModalPopupExtender.Hide();
            if(OwnerChanged != null)
            {
                OwnerChanged();
            }
        }

        protected void ShowChangeOwnedByButton_Click(object sender, EventArgs e)
        {
            BuildUserList();
            ChangeOwnedByModalPopupExtender.Show();
            ChangeOwnedByDropDownList.Focus();
        }
    }
}
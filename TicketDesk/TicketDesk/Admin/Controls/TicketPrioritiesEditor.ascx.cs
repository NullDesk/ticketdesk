using System;
using System.Collections.Generic;
using AjaxControlToolkit;
using TicketDesk.Engine;
using System.Web.UI.WebControls;

namespace TicketDesk.Admin.Controls
{
    public partial class TicketPrioritiesEditor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = string.Empty;
        }

        private List<string> OrderedPriorities
        {
            get
            {
                object o = ViewState["OrderedPriorities"];
                return (o == null) ? GetPrioritiesForViewStateFromDb() : (List<string>)o;
            }
            set
            {
                ViewState["OrderedPriorities"] = value;
            }
        }

        private List<string> GetPrioritiesForViewStateFromDb()
        {
            List<string> ret = new List<string>(SettingsManager.PrioritiesList);
            OrderedPriorities = ret;
            return ret;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TicketPrioritiesOrderList.DataSource = OrderedPriorities;
            TicketPrioritiesOrderList.DataBind();
        }

        protected void TicketPrioritiesOrderList_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            TicketPrioritiesOrderList.EditItemIndex = -1;
            string itemToMove = OrderedPriorities[e.OldIndex];
            OrderedPriorities.Remove(itemToMove);
            OrderedPriorities.Insert(e.NewIndex, itemToMove);

            SettingsManager.PrioritiesList = OrderedPriorities.ToArray();
        }

        protected void TicketPrioritiesOrderList_InsertCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("AddPriorityNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {
                OrderedPriorities.Add(tb.Text);
                SettingsManager.PrioritiesList = OrderedPriorities.ToArray();
            }
        }

        protected void TicketPrioritiesOrderList_UpdateCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("PriorityNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {

                SettingChangeResult result = SettingsManager.RenamePriority(OrderedPriorities[e.Item.ItemIndex], tb.Text);
                OrderedPriorities = new List<string>(SettingsManager.PrioritiesList);
                TicketPrioritiesOrderList.EditItemIndex = -1;
                switch(result)
                {
                    case SettingChangeResult.Merge:
                        Message.Text = "Ticket priority has been merged with an existing priority.";
                        break;
                    case SettingChangeResult.Failure:
                        Message.Text = "No changes saved.";
                        break;
                    default:
                        //do nothing, change will be apparent from updated order list
                        break;
                }

            }
        }
    }
}
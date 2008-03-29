using System;
using System.Collections.Generic;
using AjaxControlToolkit;
using TicketDesk.Engine;
using System.Web.UI.WebControls;

namespace TicketDesk.Admin.Controls
{
    public partial class TicketTypesEditor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = string.Empty;
        }

        private List<string> OrderedTypes
        {
            get
            {
                object o = ViewState["OrderedTypes"];
                return (o == null) ? GetTypesForViewStateFromDb() : (List<string>)o;
            }
            set
            {
                ViewState["OrderedTypes"] = value;
            }
        }

        private List<string> GetTypesForViewStateFromDb()
        {
            List<string> ret = new List<string>(SettingsManager.TicketTypesList);
            OrderedTypes = ret;
            return ret;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TicketTypesOrderList.DataSource = OrderedTypes;
            TicketTypesOrderList.DataBind();
        }

        protected void TicketTypesOrderList_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            TicketTypesOrderList.EditItemIndex = -1;
            string itemToMove = OrderedTypes[e.OldIndex];
            OrderedTypes.Remove(itemToMove);
            OrderedTypes.Insert(e.NewIndex, itemToMove);

            SettingsManager.TicketTypesList = OrderedTypes.ToArray();
        }

        protected void TicketTypesOrderList_InsertCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("AddTypeNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {
                OrderedTypes.Add(tb.Text);
                SettingsManager.TicketTypesList = OrderedTypes.ToArray();
            }
        }

        protected void TicketTypesOrderList_UpdateCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("TypeNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {
                SettingChangeResult result = SettingsManager.RenameTicketType(OrderedTypes[e.Item.ItemIndex], tb.Text);
                OrderedTypes = new List<string>(SettingsManager.TicketTypesList);
                TicketTypesOrderList.EditItemIndex = -1;
                switch(result)
                {
                    case SettingChangeResult.Merge:
                        Message.Text = "Ticket type has been merged with an existing type.";
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
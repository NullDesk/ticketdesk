using System;
using System.Collections.Generic;
using AjaxControlToolkit;
using TicketDesk.Engine;
using System.Web.UI.WebControls;

namespace TicketDesk.Admin.Controls
{
    public partial class TicketCategoriesEditor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = string.Empty;
        }

        private List<string> OrderedCategories
        {
            get
            {
                object o = ViewState["OrderedCategories"];
                return (o == null) ? GetCategoriesForViewStateFromDb() : (List<string>)o;
            }
            set
            {
                ViewState["OrderedCategories"] = value;
            }
        }

        private List<string> GetCategoriesForViewStateFromDb()
        {
            List<string> ret = new List<string>(SettingsManager.CategoriesList);
            OrderedCategories = ret;
            return ret;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TicketCategoriesOrderList.DataSource = OrderedCategories;
            TicketCategoriesOrderList.DataBind();
        }

        protected void TicketCategoriesOrderList_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            TicketCategoriesOrderList.EditItemIndex = -1;
            string itemToMove = OrderedCategories[e.OldIndex];
            OrderedCategories.Remove(itemToMove);
            OrderedCategories.Insert(e.NewIndex, itemToMove);

            SettingsManager.CategoriesList = OrderedCategories.ToArray();
        }

        protected void TicketCategoriesOrderList_InsertCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("AddCategoryNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {
                OrderedCategories.Add(tb.Text);
                SettingsManager.CategoriesList = OrderedCategories.ToArray();
            }
        }

        protected void TicketCategoriesOrderList_UpdateCommand(object sender, ReorderListCommandEventArgs e)
        {
            TextBox tb = (TextBox)e.Item.FindControl("CategoryNameTextBox");
            if(!string.IsNullOrEmpty(tb.Text.Trim()))
            {
                SettingChangeResult result = SettingsManager.RenameCategory(OrderedCategories[e.Item.ItemIndex], tb.Text);
                OrderedCategories = new List<string>(SettingsManager.CategoriesList);
                TicketCategoriesOrderList.EditItemIndex = -1;
                switch(result)
                {
                    case SettingChangeResult.Merge:
                        Message.Text = "Ticket category has been merged with an existing category.";
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
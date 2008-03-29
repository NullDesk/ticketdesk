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
using System.Collections.Generic;
using AjaxControlToolkit;

namespace TicketDesk
{
    /// <summary>
    /// Test of using AjaxToolkit ReorderList control with simple array. 
    /// The technique is pretty much the same for ArrayList and Generic List.
    /// </summary>
    public partial class ReorderTest : System.Web.UI.Page
    {
        private string[] OrderItems
        {
            get
            {
                object items = ViewState["OrderItems"];
                if(items == null) // items are not in viewstate, read from datastore instead
                {
                    items = GetOrderItemsFromDb(); //get values from the data store
                    ViewState["OrderItems"] = items;//shove into viewstate
                }
                return (string[])items; 
            }
            set
            {
                ViewState["OrderItems"] = value;
            }
        }

        private string[] GetOrderItemsFromDb()
        {
            string[] ret = new string[] { "Item 1", "Item 2", "Item 3" };//dummy up items, instead of using a read DB
            return ret;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = string.Empty;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // doing this at PreRender so we don't have to worry about when/if 
            //   we should bind based on if it's a postback or callback and what not.
            OrderList.DataSource = OrderItems;
            OrderList.DataBind();
        }

        protected void SaveOrder_Click(object sender, EventArgs e)
        {
            //normally you'd save to reordered list to the DB or whatever perm storage you were using
            foreach(string s in OrderItems)
            {
                Message.Text = Message.Text + s + "<br />";//should output the re-ordered values in the correct order
                
            }
        }

        protected void OrderList_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            string[] items = OrderItems;
            List<string> list = new List<string>(OrderItems);//using a list for the reordering out of convienience
            string itemToMove = list[e.OldIndex];
            list.Remove(itemToMove);
            list.Insert(e.NewIndex, itemToMove);
            OrderItems = list.ToArray();
            //you could save this to the DB now, but this example uses a save button to batch up changes
        }
    }
}

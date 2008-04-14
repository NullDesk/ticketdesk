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
using TicketDesk.Engine;
using System.Collections.Generic;
using AjaxControlToolkit;

namespace TicketDesk.Controls
{
    public partial class ComplexSortEditor : System.Web.UI.UserControl
    {
        UserSortExpressions sortExpressions;
        UserSort sort;

        private TicketList ticketListControl;
        public TicketList TicketListControl
        {
            get
            {
                return ticketListControl;
            }
            set
            {
                ticketListControl = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ticketListControl.ColumnSortChanged += new EventHandler(ticketListControl_ColumnSortChanged);
            InitSorts();

        }

        void ticketListControl_ColumnSortChanged(object sender, EventArgs e)
        {
            ViewState.Remove("OrderedSorts");
            InitSorts();
        }

        private void InitSorts()
        {
            sortExpressions = new UserSortExpressions();
            sort = sortExpressions.GetSortExpression(TicketListControl.DataSourceID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DisplaySort.Text = GetFriendlyDisplaySort();

            TicketSortOrderList.DataSource = GetFriendlyOrderedSorts();
            TicketSortOrderList.DataBind();
        }

        private List<string> GetFriendlyOrderedSorts()
        {
            List<string> friendlyOrderedSorts = new List<string>();
            foreach(string oSort in OrderedSorts)
            {
                friendlyOrderedSorts.Add(TicketListControl.GetFriendlyColumnNameForDataField(oSort));
            }
            return friendlyOrderedSorts;
        }

        private string GetFriendlyDisplaySort()
        {
            string[] expr = sort.GetSortColumnExpressions();
            List<string> friendlyNamesList = new List<string>();
            foreach(string sortCol in expr)
            {
                friendlyNamesList.Add(TicketListControl.GetFriendlyColumnNameForDataField(sortCol));
            }
            string s = string.Join(", ", friendlyNamesList.ToArray());
            return s;
        }


        #region sorting popup


        private List<string> OrderedSorts
        {
            get
            {
                object o = ViewState["OrderedSorts"];
                return (o == null) ? GetSortsViewStateFromProfile() : (List<string>)o;
            }
            set
            {
                ViewState["OrderedSorts"] = value;
            }
        }

        private List<string> GetSortsViewStateFromProfile()
        {
            List<string> s = new List<string>(sort.GetSortColumnExpressions());
            OrderedSorts = s;
            return s;
        }



        protected void TicketSortOrderList_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            TicketSortOrderList.EditItemIndex = -1;
            string itemToMove = OrderedSorts[e.OldIndex];
            OrderedSorts.Remove(itemToMove);
            OrderedSorts.Insert(e.NewIndex, itemToMove);
            SaveSort();
        }

        private void SaveSort()
        {
            if(OrderedSorts.Count > 0)
            {
                string newSort = string.Join(",", OrderedSorts.ToArray());
                string newSortExpression = UserSortExpressions.GetTruncatedSortExpression(newSort);
                SortDirection newSortDirection = UserSortExpressions.GetDirectionFromSortExpression(newSort);
                sortExpressions.SetSortExpression(TicketListControl.DataSourceID, newSortExpression, newSortDirection);
                InitSorts();
                ticketListControl.ReSortList();
            }
        }

        protected void TicketSortOrderList_ItemCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            if(e.CommandName == "direction")
            {
                string sort = OrderedSorts[e.Item.DataItemIndex];
                string truncSort = UserSortExpressions.GetTruncatedSortExpression(sort);
                SortDirection oldDir = UserSortExpressions.GetDirectionFromSortExpression(sort);
                SortDirection newDir = (oldDir == SortDirection.Ascending)? SortDirection.Descending : SortDirection.Ascending;
                OrderedSorts[e.Item.DataItemIndex] = string.Format("{0} {1}", truncSort, (newDir == SortDirection.Ascending) ? "ASC" : "DESC");
                SaveSort();
            }
        }

        protected void TicketSortOrderList_InsertCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            DropDownList ColumnsDropDownList = (DropDownList)e.Item.FindControl("ColumnsDropDownList");
            CheckBox AddSortDescendingCheckBox = (CheckBox)e.Item.FindControl("AddSortDescendingCheckBox");
            OrderedSorts.Add(string.Format("{0} {1}", ColumnsDropDownList.SelectedValue, (AddSortDescendingCheckBox.Checked) ? "DESC" : "ASC"));
            ColumnsDropDownList.DataBind();
            SaveSort();
        }


        protected bool EnableSortDelete()
        {
            return OrderedSorts.Count > 1;
        }



        protected void TicketSortOrderList_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            if(OrderedSorts.Count > 1)
            {
                OrderedSorts.RemoveAt(e.Item.DataItemIndex);
                SaveSort();
            }
        }

        
        #region Insert Sort Column DropDown

        protected void ColumnsDropDownList_PreRender(object sender, EventArgs e)
        {
                DropDownList dl = (DropDownList)sender;
                dl.SelectedIndex = -1;
                dl.DataSource = GetUnusedSortableColumns();
                dl.DataTextField = "Key";
                dl.DataValueField = "Value";
                dl.DataBind();
                if(dl.Items.Count < 1)
                {
                    dl.NamingContainer.Visible = false;
                }
            
        }

        private Dictionary<string, string> GetUnusedSortableColumns()
        {
            var friendlyOrderedSortsNoDirection = from fos in GetFriendlyOrderedSorts()
                                       select UserSortExpressions.GetTruncatedSortExpression(fos);
            
            var friendlySortables = from s in TicketListControl.GetAllSortableDataFields()
                                    let fs = ticketListControl.GetFriendlyColumnNameForDataField(s)
                                    where !friendlyOrderedSortsNoDirection.Contains(fs)
                                    select new System.Collections.Generic.KeyValuePair<string,string>(fs,s);

            return friendlySortables.ToDictionary(k => k.Key, t => t.Value);
        }



        #endregion



        #endregion

        
    }
}
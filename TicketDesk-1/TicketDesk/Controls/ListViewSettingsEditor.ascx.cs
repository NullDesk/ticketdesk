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
using System.Web;
using TicketDesk.Engine;
using TicketDesk.Engine.ListView;
using System.Linq;
using System.Web.UI.WebControls;
using TicketDesk.Engine.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace TicketDesk.Controls
{
    public partial class ListViewSettingsEditor : System.Web.UI.UserControl
    {
        private string _listName;
        private ListViewSettingsCollection userSettings = ListViewSettingsCollection.GetSettingsForUser();
        private ListViewSettings listSettings;

        public event EventHandler SettingsChanged;



        public string ListName
        {
            get { return _listName; }
            set { _listName = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ListName))
            {
                throw new ApplicationException("List Name was not supplied to ListViewSettings user control");
            }

            listSettings = userSettings.GetSettingsForList(ListName);
        }

        public void OnSettingsChanged()
        {

            EventHandler evt = SettingsChanged;
            if (evt != null)
            {
                evt(this, EventArgs.Empty);
            }
        }


        #region status list

        protected void StatusList_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                StatusList.ClearSelection();
                ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "CurrentStatus");
                if (fColumn != null)
                {
                    if (!fColumn.EqualityComparison.Value)//only the "open" setting uses not equals comparisons
                    {
                        StatusList.Items.FindByValue("open").Selected = true;
                    }
                    else
                    {
                        StatusList.Items.FindByValue(fColumn.ColumnValue).Selected = true;
                    }
                }
                else
                {
                    StatusList.Items.FindByValue("any").Selected = true;
                }


                StatusList.Enabled = !listSettings.DisabledFilterColumNames.Contains("CurrentStatus");
            }
        }


        protected void StatusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "CurrentStatus");

            if (StatusList.SelectedValue == "any")
            {
                if (fColumn != null)
                {
                    listSettings.FilterColumns.Remove(fColumn);
                }
            }
            else
            {
                bool equality = (StatusList.SelectedValue != "open");
                if (fColumn == null)
                {
                    fColumn = new ListViewFilterColumn("CurrentStatus");
                    listSettings.FilterColumns.Add(fColumn);
                }

                fColumn.EqualityComparison = equality;
                fColumn.ColumnValue = (StatusList.SelectedValue == "open") ? "Closed" : StatusList.SelectedValue;

            }
            userSettings.Save();
            OnSettingsChanged();
        }

        #endregion

        #region page size list

        protected void PageSizeList_OnLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ListItem li = PageSizeList.Items.FindByValue(listSettings.ItemsPerPage.ToString());

                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        protected void PageSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSettings.ItemsPerPage = Convert.ToInt32(PageSizeList.SelectedValue);
            userSettings.Save();
            OnSettingsChanged();
        }

        #endregion

        #region staff lists (owner/assigned)

        protected void OwnedStaffUserList_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                OwnedStaffUserList.DataSource = SecurityManager.GetTicketSubmitterUsers();
                OwnedStaffUserList.DataBind();

                OwnedStaffUserList.ClearSelection();
                ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "Owner");
                if (fColumn != null)
                {
                    var item = OwnedStaffUserList.Items.FindByValue(fColumn.ColumnValue);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    OwnedStaffUserList.Items.FindByValue("anyone").Selected = true;
                }
                OwnedStaffUserList.Enabled = !listSettings.DisabledFilterColumNames.Contains("Owner");
            }
        }

        protected void OwnedStaffUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "Owner");

            if (OwnedStaffUserList.SelectedValue == "anyone")
            {
                if (fColumn != null)
                {
                    listSettings.FilterColumns.Remove(fColumn);
                }
            }
            else
            {
                if (fColumn == null)
                {
                    fColumn = new ListViewFilterColumn("Owner");
                    listSettings.FilterColumns.Add(fColumn);
                }

                fColumn.EqualityComparison = true;
                fColumn.ColumnValue = OwnedStaffUserList.SelectedValue;

            }
            userSettings.Save();
            OnSettingsChanged();
        }

        protected void AssignedStaffUserList_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AssignedStaffUserList.DataSource = SecurityManager.GetHelpDeskUsers();

                AssignedStaffUserList.DataBind();


                AssignedStaffUserList.ClearSelection();
                ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "AssignedTo");
                if (fColumn != null)
                {
                    if (fColumn.ColumnValue == null)//the "unassigned" setting uses null
                    {
                        AssignedStaffUserList.Items.FindByValue("unassigned").Selected = true;
                        
                    }
                    else
                    {
                        var item = AssignedStaffUserList.Items.FindByValue(fColumn.ColumnValue);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }
                else
                {
                    AssignedStaffUserList.Items.FindByValue("anyone").Selected = true;
                }
                AssignedStaffUserList.Enabled = !listSettings.DisabledFilterColumNames.Contains("AssignedTo");
            }
        }

        protected void AssignedStaffUserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewFilterColumn fColumn = listSettings.FilterColumns.SingleOrDefault(fc => fc.ColumnName == "AssignedTo");

            if (AssignedStaffUserList.SelectedValue == "anyone")
            {
                if (fColumn != null)
                {
                    listSettings.FilterColumns.Remove(fColumn);
                }
            }
            else
            {
                if (fColumn == null)
                {
                    fColumn = new ListViewFilterColumn("AssignedTo");
                    listSettings.FilterColumns.Add(fColumn);
                }


                if (AssignedStaffUserList.SelectedValue == "unassigned")
                {
                    fColumn.EqualityComparison = null;
                    fColumn.ColumnValue = null;
                }
                else
                {
                    fColumn.EqualityComparison = true;
                    fColumn.ColumnValue = AssignedStaffUserList.SelectedValue;
                }
            }
            userSettings.Save();
            OnSettingsChanged();
        }

        #endregion

        #region advanced sort editor

        protected void AdvancedSortOrderList_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            ListViewSortColumn column = listSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == e.CommandArgument.ToString());
            if (column != null)
            {
                listSettings.SortColumns.Remove(column);
            }

            SaveAdvancedSortChanges();
        }

        protected void AdvancedSortOrderList_InsertCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            DropDownList ColumnsDropDownList = (DropDownList)e.Item.FindControl("AddSortColumnsList");
            CheckBox AddColumnToSortDescendingCheckBox = (CheckBox)e.Item.FindControl("AddColumnToSortDescendingCheckBox");
            ColumnSortDirection newDirection = (AddColumnToSortDescendingCheckBox.Checked) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
            ListViewSortColumn newSortColumn = new ListViewSortColumn(ColumnsDropDownList.SelectedValue, newDirection);
            listSettings.SortColumns.Add(newSortColumn);

            SaveAdvancedSortChanges();
        }

        protected void AdvancedSortOrderList_ItemCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            if (e.CommandName == "direction")
            {
                ListViewSortColumn column = listSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == e.CommandArgument.ToString());
                column.SortDirection = (column.SortDirection == ColumnSortDirection.Ascending) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;

                SaveAdvancedSortChanges();
            }
        }

        protected void AdvancedSortOrderList_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            AdvancedSortOrderList.EditItemIndex = -1;
            ListViewSortColumn colToMove = listSettings.SortColumns[e.OldIndex];
            listSettings.SortColumns.Remove(colToMove);
            listSettings.SortColumns.Insert(e.NewIndex, colToMove);

            SaveAdvancedSortChanges();
        }

        protected void AdvancedSortOrderList_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindAdvancedSortOrderList();
            }
        }

        public void BindAdvancedSortOrderList()
        {
            AdvancedSortOrderList.DataSource = listSettings.SortColumns;
            AdvancedSortOrderList.DataBind();
            DropDownList ColumnsDropDownList = (DropDownList)AdvancedSortOrderList.GetControl("AddSortColumnsList", true);
            if (ColumnsDropDownList != null)
            {
                BindAddSortColumnsList(ColumnsDropDownList);
            }
        }

        private void BindAddSortColumnsList(DropDownList list)
        {
            //get a dictionary of friendly column names keyed by the column's actual name;
            Dictionary<string, string> columnNames = new Dictionary<string, string>();
            Type t = typeof(Ticket);
            SortableFields sortAttr = (SortableFields)Attribute.GetCustomAttribute(t, typeof(SortableFields));

            foreach (string colName in sortAttr.SortableColumnNames)
            {
                if (listSettings.SortColumns.Count(c => c.ColumnName == colName) < 1)// eliminate columns already in sort
                {
                    columnNames.Add(colName.ConvertPascalCaseToFriendlyString(), colName);
                }
            }
            list.DataSource = columnNames;
            list.DataBind();
        }

        private void SaveAdvancedSortChanges()
        {
            userSettings.Save();
            BindAdvancedSortOrderList();
            OnSettingsChanged();
        }

        protected bool EnableSortItemDelete()
        {
            return (listSettings.SortColumns.Count > 1);
        }

        protected string GetSortItemDirectionImage(ColumnSortDirection direction)
        {
            return string.Format("~/Controls/Images/{0}.png", (direction == ColumnSortDirection.Ascending) ? "up" : "down");
        }

        #endregion
    }
}
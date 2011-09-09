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
using System.Collections.Generic;

namespace TicketDesk.Engine.ListView
{
    /// <summary>
    /// Represents a user's settings for a particular list view
    /// </summary>
    public class ListViewSettings
    {
        private string _listViewName;
        private string _listViewDisplayName;

        private int _listViewDisplayOrder;

       
        private int _itemsPerPage = 20;
        private List<ListViewSortColumn> _sortColumns;
        private List<ListViewFilterColumn> _filterColumns;
        private List<string> _disabledFilterColumNames;

        

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSettings"/> class.
        /// </summary>
        /// <remarks>
        /// Required for serialization by the profile provider 
        /// </remarks>
        public ListViewSettings()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSetting"/> class with a default sort and filter.
        /// </summary>
        /// <param name="listViewName">Name of the list view.</param>
        public ListViewSettings(string listViewName, string listViewDisplayName, int listViewDisplayOrder, bool includeClosedTickets)
        {
            _listViewName = listViewName;
            ListViewDisplayName = listViewDisplayName;
            ListViewDisplayOrder = listViewDisplayOrder;
            SortColumns = new List<ListViewSortColumn>();
            SortColumns.Add(new ListViewSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            FilterColumns = new List<ListViewFilterColumn>();
            if(!includeClosedTickets)
            {
                FilterColumns.Add(new ListViewFilterColumn("CurrentStatus", false, "Closed"));
            }
            DisabledFilterColumNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSetting"/> class.
        /// </summary>
        /// <param name="listViewName">Name of the list view.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="sortColumns">The sort columns.</param>
        /// <param name="filterColumns">The filter columns.</param>
        /// <param name="disabledFilterColumnNames">The disabled filter column names.</param>
        public ListViewSettings(string listViewName, string listViewDisplayName, int listViewDisplayOrder, int itemsPerPage, List<ListViewSortColumn> sortColumns, List<ListViewFilterColumn> filterColumns, List<string> disabledFilterColumnNames)
        {
            _listViewName = listViewName;
            ListViewDisplayName = listViewDisplayName;
            ListViewDisplayOrder = listViewDisplayOrder;
            ItemsPerPage = itemsPerPage;
            SortColumns = sortColumns;
            FilterColumns = filterColumns;
            DisabledFilterColumNames = (disabledFilterColumnNames) ?? new List<string>();
        }

        /// <summary>
        /// Gets the name of the list view.
        /// </summary>
        /// <value>The name of the list view.</value>
        public string ListViewName
        {
            get { return _listViewName; }
            set { _listViewName = value; }
        }


        /// <summary>
        /// Gets or sets the display name of the list view.
        /// </summary>
        /// <value>The display name of the list view.</value>
        public string ListViewDisplayName
        {
            get { return _listViewDisplayName; }
            set { _listViewDisplayName = value; }
        }

        /// <summary>
        /// Gets or sets the list display order.
        /// </summary>
        /// <value>The list display order.</value>
        public int ListViewDisplayOrder
        {
            get { return _listViewDisplayOrder; }
            set { _listViewDisplayOrder = value; }
        }

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set { _itemsPerPage = value; }
        }

        /// <summary>
        /// Gets or sets the sort columns.
        /// </summary>
        /// <value>The sort columns in the order they should be sorted by.</value>
        public List<ListViewSortColumn> SortColumns
        {
            get { return _sortColumns; }
            set { _sortColumns = value; }
        }

        /// <summary>
        /// Gets or sets the filter columns.
        /// </summary>
        /// <value>The filter columns.</value>
        public List<ListViewFilterColumn> FilterColumns
        {
            get { return _filterColumns; }
            set { _filterColumns = value; }
        }

        /// <summary>
        /// Gets or sets the disabled filter colum names.
        /// </summary>
        /// <value>The disabled filter colum names.</value>
        public List<string> DisabledFilterColumNames
        {
            get { return _disabledFilterColumNames; }
            set { _disabledFilterColumNames = value; }
        }
    }
}

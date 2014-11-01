// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Models
{

    /// <summary>
    /// Represents a user's settings for a particular list view
    /// </summary>
    public class TicketCenterListSettings
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSettings"/> class.
        /// </summary>
        /// <remarks>
        /// Required for serialization by the profile provider 
        /// </remarks>
        public TicketCenterListSettings()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSetting"/> class with a default sort and filter.
        /// </summary>
        /// <param name="listViewName">Name of the list view.</param>
        public TicketCenterListSettings(string listName, string listDisplayName, int listMenuDisplayOrder, bool includeClosedTickets)
        {
            ListName = listName;
            ListDisplayName = listDisplayName;
            ListMenuDisplayOrder = listMenuDisplayOrder;
            SortColumns = new List<TicketListSortColumn>();
            SortColumns.Add(new TicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            FilterColumns = new List<TicketListFilterColumn>();
            if (!includeClosedTickets)
            {
                FilterColumns.Add(new TicketListFilterColumn("CurrentStatus", false, "Closed"));
            }
            DisabledFilterColumNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketCenterListSettings"/> class.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listDisplayName">Display name of the list.</param>
        /// <param name="listMenuDisplayOrder">The list menu display order.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="sortColumns">The sort columns.</param>
        /// <param name="filterColumns">The filter columns.</param>
        /// <param name="disabledFilterColumnNames">The disabled filter column names.</param>
        public TicketCenterListSettings(string listName, string listDisplayName, int listMenuDisplayOrder, int itemsPerPage, List<TicketListSortColumn> sortColumns, List<TicketListFilterColumn> filterColumns, List<string> disabledFilterColumnNames)
        {
            ListName = listName;
            ListDisplayName = listDisplayName;
            ListMenuDisplayOrder = listMenuDisplayOrder;
            ItemsPerPage = itemsPerPage;
            SortColumns = sortColumns;
            FilterColumns = filterColumns;
            DisabledFilterColumNames = (disabledFilterColumnNames) ?? new List<string>();
        }

        /// <summary>
        /// Gets the name of the list view.
        /// </summary>
        /// <value>The name of the list view.</value>
        public string ListName { get; set; }



        /// <summary>
        /// Gets or sets the display name of the list view.
        /// </summary>
        /// <value>The display name of the list view.</value>
        public string ListDisplayName { get; set; }


        /// <summary>
        /// Gets or sets the list display order.
        /// </summary>
        /// <value>The list display order.</value>
        public int ListMenuDisplayOrder { get; set; }


        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage { get; set; }


        /// <summary>
        /// Gets or sets the sort columns.
        /// </summary>
        /// <value>The sort columns in the order they should be sorted by.</value>
        public List<TicketListSortColumn> SortColumns { get; set; }


        /// <summary>
        /// Gets or sets the filter columns.
        /// </summary>
        /// <value>The filter columns.</value>
        public List<TicketListFilterColumn> FilterColumns { get; set; }


        /// <summary>
        /// Gets or sets the disabled filter column names.
        /// </summary>
        /// <value>The disabled filter column names.</value>
        public List<string> DisabledFilterColumNames { get; set; }

        /// <summary>
        /// Changes the preferences to filter by the specified assigned user.
        /// </summary>
        /// <param name="assigned">The assigned user to filter by.</param>
        public void ChangeAssignedFilter(string assigned)
        {
            if (!string.IsNullOrEmpty(assigned))
            {
                var fColumn = FilterColumns.SingleOrDefault(fc => fc.ColumnName == "AssignedTo");

                if (assigned == "anyone")
                {
                    if (fColumn != null)
                    {
                        FilterColumns.Remove(fColumn);
                    }
                }
                else
                {
                    if (fColumn == null)
                    {
                        fColumn = new TicketListFilterColumn("AssignedTo");
                        FilterColumns.Add(fColumn);
                    }

                    if (assigned == "unassigned")
                    {
                        fColumn.UseEqualityComparison = null;
                        fColumn.ColumnValue = null;
                    }
                    else
                    {
                        fColumn.UseEqualityComparison = true;
                        fColumn.ColumnValue = assigned;
                    }
                }
            }
        }

        /// <summary>
        /// Changes the preferences to filter by the specified owner.
        /// </summary>
        /// <param name="ownerValue">The owner name to filter by.</param>
        public void ChangeOwnerFilter(string ownerValue)
        {
            if (!string.IsNullOrEmpty(ownerValue))
            {
                var fColumn = FilterColumns.SingleOrDefault(fc => fc.ColumnName == "Owner");

                if (ownerValue == "anyone")
                {
                    if (fColumn != null)
                    {
                        FilterColumns.Remove(fColumn);
                    }
                }
                else
                {
                    if (fColumn == null)
                    {
                        fColumn = new TicketListFilterColumn("Owner");
                        FilterColumns.Add(fColumn);
                    }

                    fColumn.UseEqualityComparison = true;
                    fColumn.ColumnValue = ownerValue;
                }
            }
        }


        /// <summary>
        /// Changes the preferences to filter by the specified current status.
        /// </summary>
        /// <param name="statusValue">The status value to filter by.</param>
        public void ChangeCurrentStatusFilter(string statusValue)
        {
            if (!string.IsNullOrEmpty(statusValue))
            {
                var fColumn = FilterColumns.SingleOrDefault(fc => fc.ColumnName == "CurrentStatus");

                if (statusValue == "any")
                {
                    if (fColumn != null)
                    {
                        FilterColumns.Remove(fColumn);
                    }
                }
                else
                {
                    bool equality = (statusValue != "open");
                    if (fColumn == null)
                    {
                        fColumn = new TicketListFilterColumn("CurrentStatus");
                        FilterColumns.Add(fColumn);
                    }

                    fColumn.UseEqualityComparison = equality;
                    fColumn.ColumnValue = (statusValue == "open") ? "closed" : statusValue;
                }
            }
        }
    }
}


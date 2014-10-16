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

using System.Collections.Generic;
using System.Linq;

namespace TicketDesk.Domain.Models
{

    /// <summary>
    /// Represents a user's settings for a particular list view
    /// </summary>
    public class UserTicketListSetting
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListSetting"/> class.
        /// </summary>
        /// <remarks>
        /// Required for serialization by the profile provider 
        /// </remarks>
        public UserTicketListSetting()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListSetting" /> class with a default sort and filter.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listDisplayName">Display name of the list.</param>
        /// <param name="listMenuDisplayOrder">The list menu display order.</param>
        /// <param name="includeClosedTickets">if set to <c>true</c> [include closed tickets].</param>
        public UserTicketListSetting(string listName, string listDisplayName, int listMenuDisplayOrder, bool includeClosedTickets)
        {
            ListName = listName;
            ListDisplayName = listDisplayName;
            ListMenuDisplayOrder = listMenuDisplayOrder;
            SortColumns = new List<UserTicketListSortColumn>();
            SortColumns.Add(new UserTicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            FilterColumns = new List<UserTicketListFilterColumn>();
            if (!includeClosedTickets)
            {
                FilterColumns.Add(new UserTicketListFilterColumn("CurrentStatus", false, "Closed"));
            }
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListSetting" /> class.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <param name="listDisplayName">Display name of the list.</param>
        /// <param name="listMenuDisplayOrder">The list menu display order.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="sortColumns">The sort columns.</param>
        /// <param name="filterColumns">The filter columns.</param>
        /// <param name="disabledFilterColumnNames">The disabled filter column names.</param>
        public UserTicketListSetting(string listName, string listDisplayName, int listMenuDisplayOrder, int itemsPerPage, List<UserTicketListSortColumn> sortColumns, List<UserTicketListFilterColumn> filterColumns, List<string> disabledFilterColumnNames)
        {
            ListName = listName;
            ListDisplayName = listDisplayName;
            ListMenuDisplayOrder = listMenuDisplayOrder;
            ItemsPerPage = itemsPerPage;
            SortColumns = sortColumns;
            FilterColumns = filterColumns;
            DisabledFilterColumnNames = (disabledFilterColumnNames) ?? new List<string>();
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
        public ICollection<UserTicketListSortColumn> SortColumns { get; set; }

        /// <summary>
        /// Gets or sets the filter columns.
        /// </summary>
        /// <value>The filter columns.</value>
        public ICollection<UserTicketListFilterColumn> FilterColumns { get; set; }

        /// <summary>
        /// Gets or sets the disabled filter column names.
        /// </summary>
        /// <value>The disabled filter column names.</value>
        public IEnumerable<string> DisabledFilterColumnNames { get; set; }


        public void ModifySetting(int pageSize, string currentStatus, string owner, string assignedTo)
        {
            ItemsPerPage = pageSize;

            if (!DisabledFilterColumnNames.Contains("CurrentStatus"))
            {
                ChangeCurrentStatusFilter(currentStatus);
            }
            if (!DisabledFilterColumnNames.Contains("Owner"))
            {
                ChangeOwnerFilter(owner);
            }
            if (!DisabledFilterColumnNames.Contains("AssignedTo"))
            {
                ChangeAssignedFilter(assignedTo);
            }
        }

        /// <summary>
        /// Changes the preferences to filter by the specified assigned user.
        /// </summary>
        /// <param name="assigned">The assigned user to filter by.</param>
        private void ChangeAssignedFilter(string assigned)
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
                        fColumn = new UserTicketListFilterColumn("AssignedTo");
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
        private void ChangeOwnerFilter(string ownerValue)
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
                        fColumn = new UserTicketListFilterColumn("Owner");
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
        private void ChangeCurrentStatusFilter(string statusValue)
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
                        fColumn = new UserTicketListFilterColumn("CurrentStatus");
                        FilterColumns.Add(fColumn);
                    }

                    fColumn.UseEqualityComparison = equality;
                    fColumn.ColumnValue = (statusValue == "open") ? "closed" : statusValue;
                }
            }
        }

        internal static List<UserTicketListSetting> GetDefaultListSettings()
        {
            var settings = new List<UserTicketListSetting>();

            var disableStatusColumn = new List<string> { "CurrentStatus"};

            const int disOrder = 0;
            var openSortColumns = new List<UserTicketListSortColumn>();
            var openFilterColumns = new List<UserTicketListFilterColumn>();
            openSortColumns.Add(new UserTicketListSortColumn("CurrentStatus", ColumnSortDirection.Ascending));
            openSortColumns.Add(new UserTicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            openFilterColumns.Add(new UserTicketListFilterColumn("CurrentStatus", false, "closed"));
            settings.Add(new UserTicketListSetting("opentickets", "All Open Tickets", disOrder + 1, 20, openSortColumns, openFilterColumns, disableStatusColumn));


            var historyticketsSortColumns = new List<UserTicketListSortColumn>();
            var historyticketsFilterColumns = new List<UserTicketListFilterColumn>();
            historyticketsSortColumns.Add(new UserTicketListSortColumn("LastUpdateDate", ColumnSortDirection.Descending));
            historyticketsFilterColumns.Add(new UserTicketListFilterColumn("CurrentStatus", true, "closed"));
           
            settings.Add(new UserTicketListSetting("historytickets", "Ticket History", disOrder + 2, 20, historyticketsSortColumns, historyticketsFilterColumns, disableStatusColumn));
            
            return settings;
        }
    }
}


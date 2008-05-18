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
    /// Represents a column and direction to sort a list view by
    /// </summary>
    public class ListViewSortColumn
    {
        ColumnSortDirection _sortDirection;
        string _columnName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSortColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public ListViewSortColumn(string columnName, ColumnSortDirection sortDirection)
        {
            _columnName = columnName;
            _sortDirection = sortDirection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewSortColumn"/> class.
        /// </summary>
        /// <remarks>
        /// Required for serialization by the profile provider 
        /// </remarks>
        public ListViewSortColumn() { }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>The sort direction.</value>
        public ColumnSortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        /// <summary>
        /// Gets the friendly name of the column for use in display controls.
        /// </summary>
        /// <remarks>
        /// Splits the column name into words at the boundary of upper case characters.
        /// </remarks>
        /// <value>The name of the column friendly.</value>
        public string ColumnFriendlyName
        {
            get 
            {
                return ColumnName.ConvertPascalCaseToFriendlyString();
                
            }
        }

    }
}

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
using TicketDesk.Domain.Utilities;
namespace TicketDesk.Domain.Models
{
    public class TicketListSortColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketListSortColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column to sort.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public TicketListSortColumn(string columnName, ColumnSortDirection sortDirection)
        {
            ColumnName = columnName;
            SortDirection = sortDirection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketListSortColumn"/> class.
        /// </summary>
        public TicketListSortColumn() { }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>The sort direction.</value>
        public ColumnSortDirection SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to sort.
        /// </summary>
        /// <value>The name of the column to sort.</value>
        public string ColumnName { get; set; }


        /// <summary>
        /// Gets the friendly name of the column to sort.
        /// </summary>
        /// <value>The friendly name of the column to sort.</value>
        public string ColumnFriendlyName
        {
            get
            {
                return ColumnName.ConvertPascalCaseToFriendlyString();
            }
        }
    }
}

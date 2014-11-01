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
    public class TicketListFilterColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketListFilterColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column to test.</param>
        /// <param name="useEqualityComparison">The equality comparison selection.</param>
        /// <param name="columnValue">The column value to test for.</param>
        public TicketListFilterColumn(string columnName, bool? useEqualityComparison, string columnValue)
        {
            ColumnName = columnName;
            ColumnValue = columnValue;
            UseEqualityComparison = useEqualityComparison;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketListFilterColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column to test.</param>
        public TicketListFilterColumn(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketListFilterColumn"/> class.
        /// </summary>
        public TicketListFilterColumn() { }

        /// <summary>
        /// Gets or sets an indicator of what kind of comparison should be used in the filter.
        /// </summary>
        /// <remarks>
        /// This determines how the where clause will compare the column's actual value against the filter's value.</remarks>
        /// <value>Set true use an equality comparison, otherwise set fase to use an inequality comparison.</value>
        public bool? UseEqualityComparison { get; set; }

        /// <summary>
        /// Gets or sets the name of the column to test.
        /// </summary>
        /// <value>The name of the column to test.</value>
        public string ColumnName { get; set; }


        /// <summary>
        /// Gets or sets the column value to test for.
        /// </summary>
        /// <value>The column value to test for.</value>
        public string ColumnValue { get; set; }
    }
}

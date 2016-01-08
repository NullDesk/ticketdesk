// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;

namespace TicketDesk.Domain.Model
{
    public class UserTicketListFilterColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListFilterColumn" /> class.
        /// </summary>
        /// <param name="columnName">Name of the column to test.</param>
        /// <param name="useEqualityComparison">The equality comparison selection.</param>
        /// <param name="columnValue">The column value to test for.</param>
        public UserTicketListFilterColumn(string columnName, bool? useEqualityComparison, object columnValue)
        : this(columnName, useEqualityComparison, columnValue, columnValue.GetType())
        {}

        public UserTicketListFilterColumn(string columnName, bool? useEqualityComparison, object columnValue, Type columnValueType)
        {
            ColumnName = columnName;
            ColumnValue = columnValue;
            ColumnValueType = columnValueType;
            UseEqualityComparison = useEqualityComparison;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListFilterColumn"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column to test.</param>
        public UserTicketListFilterColumn(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTicketListFilterColumn"/> class.
        /// </summary>
        public UserTicketListFilterColumn() { }

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
        public object ColumnValue { get; set; }


        /// <summary>
        /// Gets or sets the type of the column value.
        /// </summary>
        /// <remarks>
        /// The column value gets shipped to json as an integer, but esql can't deal 
        /// with the conversion automatically. To work around this, we'll store the 
        /// value type so esql can interrogate it to perform the correct type 
        /// conversions.
        /// </remarks>
        /// <value>The type of the column value.</value>
        public Type ColumnValueType { get; set; }
    }
}

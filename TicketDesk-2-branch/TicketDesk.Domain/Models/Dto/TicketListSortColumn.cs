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

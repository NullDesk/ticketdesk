using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace TicketDesk.Engine.ListView
{
    /// <summary>
    /// Represents a column and value filter for a list view
    /// </summary>
    public class ListViewFilterColumn
    {
        bool? _equalityComparison;
        string _columnName;
        string _columnValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewFilterColumn"/> class.
        /// </summary>
        /// <remarks>
        /// Used to create a filter column and set all properties during instantiation.
        /// </remarks>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="equalityComparison">if set to <c>true</c> [equality comparison].</param>
        /// <param name="columnValue">The column value.</param>
        public ListViewFilterColumn(string columnName, bool? equalityComparison, string columnValue)
        {
            ColumnName = columnName;
            ColumnValue = columnValue;
            EqualityComparison = equalityComparison;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewFilterColumn"/> class.
        /// </summary>
        /// <remarks>
        /// Used by editor to add a new column to filter by
        /// </remarks>
        /// <param name="columnName">Name of the column.</param>
        public ListViewFilterColumn(string columnName)
        {
            ColumnName = columnName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewFilterColumn"/> class.
        /// </summary>
        /// <remarks>
        /// Required for serialization by the profile provider 
        /// </remarks>
        public ListViewFilterColumn() { }

        /// <summary>
        /// Gets or sets a value indicating whether the filter tests for equality, or inequality.
        /// </summary>
        /// <value><c>true</c> if equality comparison; otherwise, <c>false</c>.</value>
        public bool? EqualityComparison
        {
            get { return _equalityComparison; }
            set { _equalityComparison = value; }
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
        /// Gets or sets the column value to test against.
        /// </summary>
        /// <value>The column value.</value>
        public string ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = value; }
        }
    }
}

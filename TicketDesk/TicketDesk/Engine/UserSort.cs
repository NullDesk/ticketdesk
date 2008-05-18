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
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;


namespace TicketDesk.Engine
{
    /// <summary>
    /// Represents a specific user sort from the user's profile
    /// </summary>
    public class UserSort
    {
        private string _fullExpression;
        public string[] _sortColumnsExpressions;
        public string[] _sortColumns;
        private string _truncatedExpression;
        private SortDirection _direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSort"/> class.
        /// </summary>
        /// <param name="fullExpression">The full expression.</param>
        public UserSort(string fullExpression)
        {
            _fullExpression = fullExpression;
            _truncatedExpression = UserSortExpressions.GetTruncatedSortExpression(fullExpression);
            _direction = UserSortExpressions.GetDirectionFromSortExpression(fullExpression);
        }
        
        /// <summary>
        /// Gets the sort column expressions.
        /// </summary>
        /// <returns></returns>
        public string[] GetSortColumnExpressions()
        {
            if(_sortColumnsExpressions == null)
            {
                _sortColumnsExpressions = _fullExpression.Split(',');
            }
            return _sortColumnsExpressions;
        }

        /// <summary>
        /// Gets the sort columns.
        /// </summary>
        /// <returns></returns>
        public string[] GetSortColumns()
        {
            List<string> retList = new List<string>();
            List<string> colExpList = new List<string>(GetSortColumnExpressions());
            foreach(string item in colExpList)
            {
                retList.Add(UserSortExpressions.GetTruncatedSortExpression(item));
            }

            return retList.ToArray();
        }

        /// <summary>
        /// Gets or sets the truncated sort expression.
        /// </summary>
        /// <value>The truncated sort expression.</value>
        public string TruncatedSortExpression
        {
            get
            {
                return _truncatedExpression;
            }
            set
            {
                _truncatedExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>The direction.</value>
        public SortDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Gets the sort direction for a particular column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public SortDirection? GetSortDirectionForColumn(string columnName)
        {
            SortDirection? dir = null;
            string colExpr = GetSortColumnExpressions().SingleOrDefault(sc => sc.StartsWith(columnName));
            if(colExpr != null)
            {
                dir = UserSortExpressions.GetDirectionFromSortExpression(colExpr);
            }
            return dir;
        }
    }
}

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
        public UserSort(string fullExpression)
        {
            _fullExpression = fullExpression;
            _truncatedExpression = UserSortExpressions.GetTruncatedSortExpression(fullExpression);
            _direction = UserSortExpressions.GetDirectionFromSortExpression(fullExpression);
        }

        public string[] _sortColumnsExpressions;
        public string[] GetSortColumnExpressions()
        {
            if(_sortColumnsExpressions == null)
            {
                _sortColumnsExpressions = _fullExpression.Split(',');
            }
            return _sortColumnsExpressions;
        }

        public string[] _sortColumns;
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

        private string _truncatedExpression;
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
        private SortDirection _direction;
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

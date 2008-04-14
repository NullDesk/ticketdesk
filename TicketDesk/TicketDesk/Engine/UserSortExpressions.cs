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

using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace TicketDesk.Engine
{
    /// <summary>
    /// Provides access to the user sort information from the user's profile.
    /// </summary>
    public class UserSortExpressions
    {
        ProfileCommon pc = new ProfileCommon();

        /// <summary>
        /// Gets the <see cref="UserSort"/> instance for the specified datasource, or the default sort.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public UserSort GetSortExpression(string dataSource)
        {
            string exp = null;
            if(SortExpressionsContainer.ContainsKey(dataSource))
            {
                exp = SortExpressionsContainer[dataSource];
            }
            else
            {
                exp = SortExpressionsContainer["Default"];
            }
            UserSort us = new UserSort(exp);
            return us;
        }

        /// <summary>
        /// Updates and save a sort expression
        /// </summary>
        /// <param name="dataSource">The datasource of the expression to change</param>
        /// <param name="expression">The new sort expression value</param>
        /// <param name="direction">The new sort direction</param>
        /// <returns>The updated <see cref="UserSort"/> instance</returns>
        public UserSort SetSortExpression(string dataSource, string expression, SortDirection direction)
        {
            string dir = (direction == SortDirection.Ascending) ? "ASC" : "DESC";
            SortExpressionsContainer[dataSource] = string.Format("{0} {1}", expression, dir);


            List<string> expressionStrings = new List<string>();
            foreach(var kvp in SortExpressionsContainer)
            {
                expressionStrings.Add(string.Format("{0}={1}", kvp.Key, kvp.Value));
            }

            pc.TicketListSortExpressions = string.Join("|", expressionStrings.ToArray());
            return GetSortExpression(dataSource);
        }

        public static string GetTruncatedSortExpression(string expression)
        {
            string retExp = expression;
            if(expression.LastIndexOf(' ') >= 0)
            {
                retExp = expression.Remove(expression.LastIndexOf(' '));
            }
            return retExp;
        }

        public static SortDirection GetDirectionFromSortExpression(string expression)
        {
            SortDirection retDir = SortDirection.Ascending;
            if(expression.LastIndexOf(' ') >= 0)
            {
                string dir = expression.Substring(expression.LastIndexOf(' ') + 1);
                retDir = (dir == "ASC") ? SortDirection.Ascending : SortDirection.Descending;
            }
            return retDir;
        }

        private Dictionary<string, string> expressionsContainer;
        private Dictionary<string, string> SortExpressionsContainer
        {
            get
            {
                if(expressionsContainer == null)
                {
                    string s = pc.TicketListSortExpressions;
                    string[] expressions = s.Split('|');
                    expressionsContainer = new Dictionary<string, string>();
                    foreach(string exp in expressions)
                    {
                        string[] expItem = exp.Split('=');
                        if(expItem.Length == 2)
                        {
                            expressionsContainer.Add(expItem[0], expItem[1]);
                        }
                    }
                }
                return expressionsContainer;
            }
        }
    }
}

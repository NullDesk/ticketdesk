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

namespace TicketDesk.Engine.Linq
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SortableFields : Attribute
    {
        private List<string> _sortableColumnNames = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableFields"/> class.
        /// </summary>
        /// <param name="sortableColumnNames">The sortable column names.</param>
        public SortableFields(params string[] sortableColumnNames)
        {
            foreach(string col in sortableColumnNames)
            {
                _sortableColumnNames.Add(col);
            }
        }
        /// <summary>
        /// Gets the sortable column names.
        /// </summary>
        /// <value>The sortable column names.</value>
        public List<string> SortableColumnNames
        {
            get { return _sortableColumnNames; }
        }
    }
}

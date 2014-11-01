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
    /// <summary>
    /// A sort direction for columns in a list view
    /// </summary>
    public enum ColumnSortDirection
    {
        /// <summary>
        /// column values should be sorted in ascending order
        /// </summary>
        Ascending,
        /// <summary>
        /// column values should be sorted in descending order
        /// </summary>
        Descending
    }
}

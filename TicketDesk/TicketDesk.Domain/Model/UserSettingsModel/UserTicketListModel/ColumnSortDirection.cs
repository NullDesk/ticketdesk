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

namespace TicketDesk.Domain.Model
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

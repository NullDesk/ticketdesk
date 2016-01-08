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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketDesk.Search.Common
{
    public interface ISearchIndexProvider
    {
        Task<bool> AddItemsToIndexAsync(IEnumerable<SearchIndexItem> items);
        Task<bool> RunIndexMaintenanceAsync();
        Task<bool> RemoveIndexAsync();
        bool IsConfigured { get; }
    }
}

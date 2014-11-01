using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search
{
    internal interface ISearchIndexManager
    {
        Task<bool> AddItemsToIndexAsync(IEnumerable<SearchQueueItem> items);
        Task<bool> RunStartupIndexMaintenanceAsync();

    }
}

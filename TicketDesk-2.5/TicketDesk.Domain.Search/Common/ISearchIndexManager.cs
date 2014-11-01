using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search
{
    internal interface ISearchIndexManager
    {
        Task<bool> AddItemsToIndexAsync(IEnumerable<SearchQueueItem> items);
        Task<bool> RunStartupIndexMaintenanceAsync();

    }
}

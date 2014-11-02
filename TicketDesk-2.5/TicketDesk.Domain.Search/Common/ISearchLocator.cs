using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search
{
    internal interface ISearchLocator
    {
        Task<IEnumerable<SearchResultItem>> SearchAsync(string searchText);
    }
}

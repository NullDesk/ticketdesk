using System.Collections.Generic;

namespace TicketDesk.Domain.Search
{
    internal interface ISearchLocator
    {
        IEnumerable<SearchResultItem> Search(string searchText);
    }
}

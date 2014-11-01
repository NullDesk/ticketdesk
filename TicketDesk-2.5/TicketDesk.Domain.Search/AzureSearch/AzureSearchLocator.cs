using System;
using System.Collections.Generic;

namespace TicketDesk.Domain.Search.AzureSearch
{
    internal class AzureSearchLocator : AzureSearchConector, ISearchLocator
    {
        private readonly string _indexName;

        internal AzureSearchLocator(string indexName)
        {
            _indexName = indexName;
        }

        public IEnumerable<SearchResultItem> Search(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}

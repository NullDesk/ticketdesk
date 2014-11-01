using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search.AzureSearch
{
    internal class AzureSearchLocator : AzureSearchConector, ISearchLocator
    {
        private readonly string indexName;

        internal AzureSearchLocator(string indexName)
        {
            this.indexName = indexName;
        }

        public IEnumerable<SearchResultItem> Search(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Search.Common;

namespace TicketDesk.Search
{
    public class SearchContextConfiguration
    {
        public SearchContextConfiguration(ISearchIndexProvider indexProvider, ISearchLocatorProvider locatorProvider)
        {
            SearchIndexProvider = indexProvider;
            SearchLocatorProvider = locatorProvider;
        }

        public ISearchIndexProvider SearchIndexProvider { get; set; }
        public ISearchLocatorProvider SearchLocatorProvider { get; set; }
    }
}

// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
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
using TicketDesk.Domain.Search.AzureSearch;
using TicketDesk.Domain.Search.Lucene;

namespace TicketDesk.Domain.Search
{
    public class TicketDeskSearchProvider
    {
        private static TicketDeskSearchProvider _instance;
        public static TicketDeskSearchProvider GetInstance(bool isAzure)
        {
            return _instance ?? (_instance = new TicketDeskSearchProvider(isAzure));
        }

        private readonly bool _isAzure;
        private readonly string _indexName;

        internal TicketDeskSearchProvider(bool isAzure)
        {
            _isAzure = isAzure;
            _indexName = "ticketdesk-searchindex";
        }

        private ISearchLocator _indexSearcher;
        internal ISearchLocator IndexSearcher
        {
            get
            {
                if (_indexSearcher == null)
                {
                    if (_isAzure)
                    {
                        _indexSearcher = new AzureSearchLocator(_indexName);
                    }
                    else
                    {
                        _indexSearcher = new LuceneSearchLocator(_indexName);
                    }
                }
                return _indexSearcher;
            }
        }

        private ISearchIndexManager _indexManager;
        internal ISearchIndexManager IndexManager
        {
            get
            {
                if (_indexManager == null)
                {
                    if (_isAzure)
                    {
                        _indexManager = new AzureIndexManager(_indexName);
                    }
                    else
                    {
                        _indexManager = new LuceneIndexManager(_indexName);
                    }

                  
                }
                return _indexManager;
            }
        }

        public async Task<bool> InitializeSearch()
        {
            return await IndexManager.RunStartupIndexMaintenanceAsync();
        }

        public async Task<bool> RemoveIndexAsync()
        {
            return await IndexManager.RemoveIndexAsync();
        }

        public async Task<IEnumerable<SearchResultItem>> SearchIndexAsync(string searchText)
        {
            return await IndexSearcher.SearchAsync(searchText);
        }

        public async Task<bool> AddItemsToIndexAsync(IEnumerable<SearchQueueItem> items)
        {
            return await IndexManager.AddItemsToIndexAsync(items);
        }

        public async Task<bool> QueueItemsForIndexingAsync(IEnumerable<SearchQueueItem> items)
        {
            //TODO: temp "poor man" solution for testing. Will be replaced by a formal queue/dequeue system
            return await AddItemsToIndexAsync(items);
            
        }
    }
}

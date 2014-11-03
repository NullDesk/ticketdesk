using System.Collections.Generic;
using System.Threading.Tasks;
using TicketDesk.Domain.Search.AzureSearch;
using TicketDesk.Domain.Search.Lucene;

namespace TicketDesk.Domain.Search
{
    public class SearchManager
    {
        private static SearchManager _instance;
        public static SearchManager GetInstance(bool isAzure)
        {
            return _instance ?? (_instance = new SearchManager(isAzure));
        }

        private readonly bool _isAzure;
        private readonly string _indexName;

        internal SearchManager(bool isAzure)
        {
            _isAzure = isAzure;
            _indexName = "ticketdesk-tickets";
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

        public async Task<bool> QueueItemsForIndexingAsync(IEnumerable<SearchQueueItem> items)
        {
            //TODO: temp "poor man" solution for testing. Will be replaced by a formal queue/dequeue system
            return await IndexManager.AddItemsToIndexAsync(items);
            
        }
    }
}

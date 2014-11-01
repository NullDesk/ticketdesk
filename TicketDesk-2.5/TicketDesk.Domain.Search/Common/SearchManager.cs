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

                    //TODO: when the full queue is coded up, this task should be performed only be process(es) which handle the dequeue operations
                    _indexManager.RunStartupIndexMaintenanceAsync();
                }
                return _indexManager;
            }
        }

        public IEnumerable<SearchResultItem> Search(string searchText)
        {
            return IndexSearcher.Search(searchText);
        }

        public async Task<bool> QueueItemsForIndexingAsync(IEnumerable<SearchQueueItem> items)
        {
            //TODO: temp "poor man" solution for testing. Will be replaced by a formal queue/dequeue system
            return await IndexManager.AddItemsToIndexAsync(items);
            
        }
    }
}

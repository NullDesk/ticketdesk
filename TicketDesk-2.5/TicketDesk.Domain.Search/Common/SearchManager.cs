using System.Collections.Generic;
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

        private readonly bool isAzure;
        private readonly string indexName;

        internal SearchManager(bool isAzure)
        {
            this.isAzure = isAzure;
            this.indexName = "ticketdesk-tickets";
        }

        private ISearchLocator indexSearcher;
        internal ISearchLocator IndexSearcher
        {
            get
            {
                if (indexSearcher == null)
                {
                    if (isAzure)
                    {
                        indexSearcher = new AzureSearchLocator(indexName);
                    }
                    else
                    {
                        indexSearcher = new LuceneSearchLocator(indexName);
                    }
                }
                return indexSearcher;            
            }
        }

        private ISearchIndexManager indexManager;
        internal ISearchIndexManager IndexManager
        {
            get
            {
                if (indexManager == null)
                {
                    if (isAzure)
                    {
                        indexManager = new AzureIndexManager(indexName);
                    }
                    else
                    {
                        indexManager = new LuceneIndexManager(indexName);
                    }
                    
                    //TODO: when the full queue is coded up, this task should be performed only be process(es) which handle the dequeue operations
                    indexManager.RunStartupIndexMaintenanceAsync();
                }
                return indexManager;
            }
        }

        public IEnumerable<SearchResultItem> Search(string searchText)
        {
            return IndexSearcher.Search(searchText);
        } 

        public bool QueueItemsForIndexing(IEnumerable<SearchQueueItem> items)
        {
            //TODO: temp "poor man" solution for testing. Will be replaced by a formal queue/dequeue system
            IndexManager.AddItemsToIndexAsync(items);
            
            return true;
        }
    }
}

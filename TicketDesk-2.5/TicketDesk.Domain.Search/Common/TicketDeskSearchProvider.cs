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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketDesk.Domain.Search.AzureSearch;
using TicketDesk.Domain.Search.Lucene;
using TicketDesk.IO;

namespace TicketDesk.Domain.Search
{
    public class TicketDeskSearchProvider
    {
        private static IQueueProvider _searchQueue;
        public static IQueueProvider SearchQueue
        {
            get
            {
                return _searchQueue ?? (_searchQueue = TicketDeskQueueStorage.GetQueue("ticketdesk-search-queue"));
            }
        }

        private readonly bool _isAzure;
        private readonly string _indexName;

        private bool hasValidAzureSearchConnection()
        {
            return (AzureSearchConector.TryGetInfoFromConnectionString() ?? AzureSearchConector.TryGetInfoFromAppSettings()) != null;
        }
        public TicketDeskSearchProvider(ApplicationSearchMode mode, string indexName)
        {
            _isAzure =
                (mode == ApplicationSearchMode.AzureSearch) ||
                (mode == ApplicationSearchMode.Auto && hasValidAzureSearchConnection());

            _indexName = indexName;
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

        public async Task<bool> InitializeSearchAsync()
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

        public async Task QueueItemsForIndexingAsync(IEnumerable<SearchQueueItem> items)
        {
            await SearchQueue.EnqueueItemsAsync(items);
        }
    }
}

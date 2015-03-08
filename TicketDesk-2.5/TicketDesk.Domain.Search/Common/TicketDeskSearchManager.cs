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
    public class TicketDeskSearchManager
    {
        private static TicketDeskSearchManager _current;
        public static TicketDeskSearchManager Current
        {
            get { return _current ?? (_current = new TicketDeskSearchManager("ticketdesk-searchindex")); }
        }

        private readonly string _indexName;
        private IQueueProvider _searchQueue;
        private ISearchLocator _indexSearcher;
        private ISearchIndexManager _indexManager;

        internal TicketDeskSearchManager(string indexName)
        {
            IsAzure = (hasValidAzureSearchConnection());
            _indexName = indexName;
        }

        public IQueueProvider SearchQueue
        {
            get
            {
                return _searchQueue ?? (_searchQueue = TicketDeskQueueStorage.GetQueue("ticket-search-queue"));
            }
        }

        public Type GetConnectorType()
        {
            return IndexManager.GetType();
        }

        public async Task<bool> RunIndexMaintenanceAsync()
        {
            return await IndexManager.RunIndexMaintenanceAsync();
        }

        public async Task<bool> InitializeSearchAsync()
        {
            return await IndexManager.RunIndexMaintenanceAsync();
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

        private bool hasValidAzureSearchConnection()
        {
            return (AzureSearchConector.TryGetInfoFromConnectionString() ?? AzureSearchConector.TryGetInfoFromAppSettings()) != null;
        }

        internal ISearchLocator IndexSearcher
        {
            get
            {
                if (_indexSearcher == null)
                {
                    if (IsAzure)
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

        internal ISearchIndexManager IndexManager
        {
            get
            {
                if (_indexManager == null)
                {
                    if (IsAzure)
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

        private bool IsAzure { get; set; }

    }
}

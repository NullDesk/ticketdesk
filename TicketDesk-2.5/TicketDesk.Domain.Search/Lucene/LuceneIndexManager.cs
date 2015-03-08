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
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Lucene.Net.Index;

namespace TicketDesk.Domain.Search.Lucene
{
    internal class LuceneIndexManager : LuceneSearchConnector, ISearchIndexManager
    {
        internal LuceneIndexManager(string indexLocation)
            : base(indexLocation)
        {
            InitWriter();
        }

        public async Task<bool> RunIndexMaintenanceAsync()
        {
            //optimize on startup
            TdIndexWriter.Optimize();
            TdIndexWriter.Commit();
            return await Task.FromResult(true);
        }

        
        public Task<bool> AddItemsToIndexAsync(IEnumerable<SearchQueueItem> items)
        {
            return Task.Run(() =>
            {
                try
                {
                    foreach (var item in items)
                    {
                        UpdateIndexForItem(item);
                    }
                    TdIndexWriter.Commit();
                } // ReSharper disable once EmptyGeneralCatchClause
                catch 
                { 
                    //TODO: log this somewhere
                }
                return true;
            });
        }

        public Task<bool> RemoveIndexAsync()
        {
            try
            {
                if (IndexLocation != "ram")
                {
                    var directoryInfo = new DirectoryInfo(IndexLocation);
                    Parallel.ForEach(directoryInfo.GetFiles(), file => file.Delete());
                }
                ShutDownWriter();
                
            } // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //TODO: log this somewhere
            }
            return Task.FromResult(true);
        }

       
        private void UpdateIndexForItem(SearchQueueItem item)
        {
            TdIndexWriter.UpdateDocument(
                new Term("id", item.Id.ToString(CultureInfo.InvariantCulture)),
                item.ToLuceneDocument());

            
        }

    }
}

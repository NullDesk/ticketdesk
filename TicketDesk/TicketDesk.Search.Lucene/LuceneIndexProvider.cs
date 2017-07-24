// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
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
using TicketDesk.Search.Common;
using System;

namespace TicketDesk.Search.Lucene
{
    public class LuceneIndexProvider : LuceneSearchConnector, ISearchIndexProvider
    {
        public LuceneIndexProvider()
            : base("ticketdesk-searchindex")
        {
            InitWriter();
        }

        /// <summary>
        /// Gets a value indicating whether this provider is correctly configured and available for use.
        /// </summary>
        /// <value><c>true</c> if this provider is available; otherwise, <c>false</c>.</value>
        public bool IsConfigured
        {
            get
            {
                //there are no configuration requriments for local lucene indexes
                return true;
            }
        }

        public async Task<bool> RunIndexMaintenanceAsync()
        {
            //optimize on startup
            TdIndexWriter.Optimize();
            TdIndexWriter.Commit();
            ShutDownWriter();
            return await Task.FromResult(true);
        }

        
        public Task<bool> AddItemsToIndexAsync(IEnumerable<SearchIndexItem> items)
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

       
        private void UpdateIndexForItem(SearchIndexItem item)
        {
            TdIndexWriter.UpdateDocument(
                new Term("id", item.Id.ToString(CultureInfo.InvariantCulture)),
                item.ToLuceneDocument());

            
        }

    }
}

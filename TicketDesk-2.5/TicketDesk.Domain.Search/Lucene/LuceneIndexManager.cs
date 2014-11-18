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
        }

        public async Task<bool> RunStartupIndexMaintenanceAsync()
        {
            return await Task.FromResult(true);//nothing to do for lucene, the indexes is built as a side-effect of adding documents
        }

        public Task<bool> AddItemsToIndexAsync(IEnumerable<SearchQueueItem> items)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var indexWriter = await GetIndexWriterAsync();
                    foreach (var item in items)
                    {
                        UpdateIndexForItem(indexWriter, item);
                    }
                    indexWriter.Optimize();
                    indexWriter.Dispose();
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
                else
                {
                    TdIndexDirectory.Dispose();
                }
            } // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //TODO: log this somewhere
            }
            return Task.FromResult(true);
        }

        private static void UpdateIndexForItem(IndexWriter indexWriter, SearchQueueItem item)
        {
            indexWriter.UpdateDocument(
                new Term("id", item.Id.ToString(CultureInfo.InvariantCulture)),
                item.ToLuceneDocument());
        }

        #region obsolete, keep a while for reference


        //public Task RebuildIndexAsync()
        //{
        //    return Task.Run(async () =>
        //    {
        //        var indexWriter = await GetIndexWriterAsync();
        //        BuildSearchIndex(TdIndexAnalyzer, indexWriter, maxItemsPerBatch);
        //    });
        //} 

        //private static void BuildSearchIndex(Analyzer indexAnalyzer, IndexWriter indexWriter, int batchSize)
        //{
        //    int count;
        //    var skip = 0;
        //    using (var context = new TicketDeskContext())
        //    {
        //        count = context.Tickets.AsNoTracking().Count();
        //    }
        //    while (skip * batchSize < count)
        //    {
        //        IndexBatch(indexWriter, skip, batchSize);
        //        skip++;
        //    }

        //    indexWriter.Optimize();

        //    //cleanup
        //    indexWriter.Dispose();
        //    indexAnalyzer.Dispose();

        //}

        //private static void IndexBatch(IndexWriter indexWriter, int skip, int batchSize)
        //{
        //    //new context each time to keep same context from keeping every item in memory as it progresses
        //    using (var context = new TicketDeskContext())
        //    {
        //        var tickets = context.Tickets.Include(t => t.TicketComments).AsNoTracking()
        //            .OrderBy(t => t.TicketId)
        //            .Skip(skip * batchSize)
        //            .Take(batchSize);
        //        foreach (var t in tickets)
        //        {
        //            UpdateIndexForTicket(indexWriter, t);
        //        }
        //        indexWriter.Commit();
        //    }
        //}



        #endregion





    }
}

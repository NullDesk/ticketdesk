using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using TicketDesk.Domain.Model.Extensions;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Model.Search
{
    public class SearchIndexer : SearchIndexManagerBase
    {
        private readonly int maxTicketsPerBatch;

        internal SearchIndexer(string indexLocation, int maxTicketsPerBatch):base(indexLocation)
        {
            this.maxTicketsPerBatch = maxTicketsPerBatch;
        }

        public Task RebuildIndexAsync()
        {
            return Task.Run(async () =>
            {
                var indexWriter = await GetIndexWriterAsync();
                BuildSearchIndex(TdIndexAnalyzer, indexWriter, maxTicketsPerBatch);
            });
        }

        internal Task UpdateIndexForTicketsAsync(params Ticket[] tickets)
        {
            return Task.Run(async () =>
            {
                var indexWriter = await GetIndexWriterAsync();
                foreach (var ticket in tickets)
                {
                    UpdateIndexForTicket(indexWriter, ticket);
                }
                indexWriter.Optimize();
                indexWriter.Dispose();
            });
        }


        #region static internals

       

        private static void BuildSearchIndex(Analyzer indexAnalyzer, IndexWriter indexWriter, int batchSize)
        {
            int count;
            var skip = 0;
            using (var context = new TicketDeskContext())
            {
                count = context.Tickets.AsNoTracking().Count();
            }
            while (skip * batchSize < count)
            {
                IndexBatch(indexWriter, skip, batchSize);
                skip++;
            }

            indexWriter.Optimize();

            //cleanup
            indexWriter.Dispose();
            indexAnalyzer.Dispose();

        }

        private static void IndexBatch(IndexWriter indexWriter, int skip, int batchSize)
        {
            //new context each time to keep same context from keeping every item in memory as it progresses
            using (var context = new TicketDeskContext())
            {
                var tickets = context.Tickets.Include(t => t.TicketComments).AsNoTracking()
                    .OrderBy(t => t.TicketId)
                    .Skip(skip * batchSize)
                    .Take(batchSize);
                foreach (var t in tickets)
                {
                    UpdateIndexForTicket(indexWriter, t);
                }
                indexWriter.Commit();
            }
        }

        private static void UpdateIndexForTicket(IndexWriter indexWriter, Ticket ticket)
        {
            indexWriter.UpdateDocument(
                new Term("id", ticket.TicketId.ToString(CultureInfo.InvariantCulture)),
                ticket.CreateSearchDocument());
        }

        #endregion
    }
}

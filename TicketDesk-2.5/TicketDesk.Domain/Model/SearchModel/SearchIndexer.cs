using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using TicketDesk.Domain.Model.Extensions;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Model.Search
{
    public class SearchIndexer : IDisposable
    {
        internal SearchIndexer(string indexLocation, int maxTicketsPerBatch)
        {
            isGenerating = false;
            MaxTicketsPerBatch = maxTicketsPerBatch;
            IndexLocation = indexLocation;
        }

        public System.Threading.Tasks.Task UpdateIndexForTicketsAsync(IEnumerable<Ticket> tickets)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DelayIfLocked();
                var indexWriter = new IndexWriter(TdIndexDirectory, TdIndexAnalyzer,
                    IndexWriter.MaxFieldLength.UNLIMITED);

                foreach (var ticket in tickets)
                {
                    UpdateIndexForTicket(indexWriter, ticket);
                }
                indexWriter.Optimize();
                indexWriter.Dispose();
            });

        }

        public System.Threading.Tasks.Task GenerateIndexAsync()
        {

            return System.Threading.Tasks.Task.Run(() =>
            {
                if (!isGenerating)
                {
                    isGenerating = true;
                    DelayIfLocked();
                    var indexWriter = new IndexWriter(TdIndexDirectory, TdIndexAnalyzer,
                        IndexWriter.MaxFieldLength.UNLIMITED);
                    BuildSearchIndex(TdIndexDirectory, TdIndexAnalyzer, indexWriter, MaxTicketsPerBatch);
                    isGenerating = false;
                }
            });
        }

        #region Internals

        private string IndexLocation { get; set; }
        private Directory tdIndexDirectory;
        private Analyzer tdIndexAnalyzer;
        private int MaxTicketsPerBatch { get; set; }
        private bool isGenerating;

        private void DelayIfLocked()
        {
            //wait up to 4 minutes for lock to clear on its own
            var delayCount = 0;
            while (IndexWriter.IsLocked(TdIndexDirectory) && delayCount++ < 16)//delay 16 times (4 mins)
            {
                Thread.Sleep(15000);
            }
            if (IndexWriter.IsLocked(TdIndexDirectory))
            {
                IndexWriter.Unlock(TdIndexDirectory);
            }
        }

        private Directory TdIndexDirectory
        {
            get
            {
                if (tdIndexDirectory == null)
                {
                    if (string.Equals(IndexLocation, "ram", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tdIndexDirectory = new RAMDirectory();
                    }
                    else
                    {
                        var dirInfo = new System.IO.DirectoryInfo(IndexLocation);
                        tdIndexDirectory = FSDirectory.Open(dirInfo);
                    }
                }
                return tdIndexDirectory;
            }

        }
        private Analyzer TdIndexAnalyzer
        {
            get
            {
                return tdIndexAnalyzer ??
                    (tdIndexAnalyzer = new StandardAnalyzer(Version.LUCENE_30));
            }
        }

        public void Dispose()
        {
            if (TdIndexAnalyzer != null) { TdIndexAnalyzer.Dispose(); }
            if (TdIndexDirectory != null) { TdIndexDirectory.Dispose(); }
        }
        #endregion

        #region static internals

        private static void CleanupDirectory(Directory directory)
        {
                if (directory is RAMDirectory)
                {
                    directory.Dispose();
                }
        }

        private static void BuildSearchIndex(Directory indexDirectory, Analyzer indexAnalyzer, IndexWriter indexWriter, int batchSize)
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
            CleanupDirectory(indexDirectory);

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

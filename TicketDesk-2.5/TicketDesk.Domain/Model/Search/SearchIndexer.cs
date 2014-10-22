using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using TicketDesk.Domain.Model.Extensions;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Model.Search
{
    public class SearchIndexer
    {
        private string IndexLocation { get; set; }
        private Directory TdIndexDirectory { get; set; }
        private Analyzer TdIndexAnalyzer { get; set; }
        private int MaxTicketsPerBatch { get; set; }
        public SearchIndexer(string indexLocation, int maxTicketsPerBatch)
        {
            MaxTicketsPerBatch = maxTicketsPerBatch;
            IndexLocation = indexLocation;
            TdIndexDirectory = FSDirectory.Open(indexLocation);
            TdIndexAnalyzer = new StandardAnalyzer(Version.LUCENE_30);
        }
        
        public void UpdateIndexForTicket(string indexLocation, Ticket ticket)
        {
            var indexWriter = new IndexWriter(TdIndexDirectory, TdIndexAnalyzer,
                    IndexWriter.MaxFieldLength.UNLIMITED)
            {
                MergeFactor = 5
            };
            UpdateIndexForTicket(indexWriter, ticket);

            System.Threading.Tasks.Task.Run(() => { indexWriter.Optimize(); indexWriter.Commit(); });
        }

        public System.Threading.Tasks.Task GenerateIndexAsync()
        {
            //don't use shared dir and analyzer for this
            var indexDirectory = FSDirectory.Open(IndexLocation);
            var indexAnalyzer = new StandardAnalyzer(Version.LUCENE_30);
            return System.Threading.Tasks.Task.Run(() => BuildSearchIndex(indexDirectory, indexAnalyzer, MaxTicketsPerBatch));
        }


        #region Internals
       

        private static readonly object genLock = new object();


        private static void BuildSearchIndex(Directory indexDirectory, Analyzer indexAnalyzer, int batchSize)
        {
            lock (genLock)
            {

                var indexWriter = new IndexWriter(indexDirectory, indexAnalyzer,
                    IndexWriter.MaxFieldLength.UNLIMITED)
                {
                    MergeFactor = 25
                };

                ProcessIndex(indexWriter, batchSize);
            }
        }

        private static void ProcessIndex(IndexWriter indexWriter, int batchSize)
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
            indexWriter.Commit();
            indexWriter.Dispose();
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
            }
        }
        private static object updateLock = new object();
        private static void UpdateIndexForTicket(IndexWriter indexWriter, Ticket ticket)
        {
            lock (updateLock)
            {
                indexWriter.UpdateDocument(
                    new Term("id", ticket.TicketId.ToString(CultureInfo.InvariantCulture)),
                    ticket.CreateSearchDocument());
            }
        }
        #endregion

     
    }



}

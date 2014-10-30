using System;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Search.Lucene
{
    public abstract class LuceneSearchConnector : IDisposable
    {
        private Directory tdIndexDirectory;
        private Analyzer tdIndexAnalyzer;
        private IndexWriter tdIndexWriter;

        private readonly string indexLocation;

        internal LuceneSearchConnector(string indexLocation)
        {
            
            this.indexLocation = indexLocation;
        }

        protected Task<IndexWriter> GetIndexWriterAsync()
        {
            return Task.Run(async () =>
            {
                if (tdIndexWriter == null)
                {
                    var delayCount = 0;
                    while (IndexWriter.IsLocked(TdIndexDirectory) && delayCount++ < 4) //delay 4 times (1 min)
                    {
                        await Task.Delay(15000); //hyperspace for 15sec
                    }
                    if (IndexWriter.IsLocked(TdIndexDirectory))
                    {
                        //directory still locked, blow away the other lock and hope there isn't 
                        //  really another writer open elsewhere
                        IndexWriter.Unlock(TdIndexDirectory);
                    }
                    tdIndexWriter = new IndexWriter(
                        TdIndexDirectory,
                        TdIndexAnalyzer,
                        IndexWriter.MaxFieldLength.UNLIMITED);
                }
                return tdIndexWriter;
            });
        }


        protected Directory TdIndexDirectory
        {
            get
            {
                if (tdIndexDirectory == null)
                {
                    if (string.Equals(indexLocation, "ram", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tdIndexDirectory = new RAMDirectory();
                    }
                    else
                    {
                        var dirInfo = new System.IO.DirectoryInfo(indexLocation);
                        tdIndexDirectory = FSDirectory.Open(dirInfo);
                    }
                }
                return tdIndexDirectory;
            }
        }
        protected Analyzer TdIndexAnalyzer
        {
            get
            {
                return tdIndexAnalyzer ?? (tdIndexAnalyzer = new StandardAnalyzer(Version.LUCENE_30));
            }
        }

        public void Dispose()
        {
            if (tdIndexWriter != null) { tdIndexWriter.Dispose(); }
            if (TdIndexAnalyzer != null) { TdIndexAnalyzer.Dispose(); }
            if (TdIndexDirectory != null) { TdIndexDirectory.Dispose(); }
        }
    }
}

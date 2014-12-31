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
using System.IO;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Search.Lucene
{
    internal abstract class LuceneSearchConnector : IDisposable
    {
        private Directory _tdIndexDirectory;
        private Analyzer _tdIndexAnalyzer;
        private IndexWriter _tdIndexWriter;

        protected string IndexLocation { get; private set; }

        internal LuceneSearchConnector(string indexLocation)
        {

            var datadir = AppDomain.CurrentDomain.GetData("DataDirectory");
            IndexLocation = datadir == null ? "ram" : Path.Combine(datadir.ToString(), indexLocation);

           
        }

        protected Task<IndexWriter> GetIndexWriterAsync()
        {
            return Task.Run(async () =>
            {
                if (_tdIndexWriter == null)
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
                    _tdIndexWriter = new IndexWriter(
                        TdIndexDirectory,
                        TdIndexAnalyzer,
                        IndexWriter.MaxFieldLength.UNLIMITED);
                }
                return _tdIndexWriter;
            });
        }


        protected Directory TdIndexDirectory
        {
            get
            {
                if (_tdIndexDirectory == null)
                {
                    if (string.Equals(IndexLocation, "ram", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _tdIndexDirectory = new RAMDirectory();
                    }
                    else
                    {
                        

                        var dirInfo = new DirectoryInfo(IndexLocation);
                        _tdIndexDirectory = FSDirectory.Open(dirInfo);
                    }
                }
                return _tdIndexDirectory;
            }
        }
        protected Analyzer TdIndexAnalyzer
        {
            get
            {
                return _tdIndexAnalyzer ?? (_tdIndexAnalyzer = new StandardAnalyzer(Version.LUCENE_30));
            }
        }

        public void Dispose()
        {
            if (_tdIndexWriter != null) { _tdIndexWriter.Dispose(); }
            if (TdIndexAnalyzer != null) { TdIndexAnalyzer.Dispose(); }
            if (TdIndexDirectory != null) { TdIndexDirectory.Dispose(); }
        }
    }
}

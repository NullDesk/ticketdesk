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

using System;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Search.Lucene
{
    public abstract class LuceneSearchConnector : IDisposable
    {
        private Directory _tdIndexDirectory;
        private Analyzer _tdIndexAnalyzer;
        private IndexWriter _tdIndexWriter;

        protected string IndexLocation { get; private set; }

        internal LuceneSearchConnector(string indexLocation)
        {
            var fi = new DirectoryInfo(indexLocation);
            if (!fi.Exists)
            {
                var datadir = AppDomain.CurrentDomain.GetData("DataDirectory");
                IndexLocation = datadir == null ? "ram" : Path.Combine(datadir.ToString(), indexLocation);
            }
            else
            {
                IndexLocation = indexLocation;
            }

        }
       
        public IndexWriter TdIndexWriter
        {
            get
            {
                if (_tdIndexWriter == null)
                {
                    InitWriter();
                }
                return _tdIndexWriter;
            }
            
        }

        public void ShutDownWriter()
        {
           _tdIndexWriter.Optimize();
            _tdIndexWriter.Dispose();
            _tdIndexDirectory.Dispose();
            _tdIndexWriter = null;
            _tdIndexDirectory = null;
        }

        private readonly object _getWriterLock = new object();
        public void InitWriter()
        {
            lock (_getWriterLock)
            {
                if (IndexWriter.IsLocked(TdIndexDirectory))
                {
                    //directory locked, blow away the lock and hope there isn't 
                    //  really another writer open elsewhere
                    IndexWriter.Unlock(TdIndexDirectory);
                }
                _tdIndexWriter = new IndexWriter(
                    TdIndexDirectory,
                    TdIndexAnalyzer,
                    IndexWriter.MaxFieldLength.UNLIMITED);
            }
        }


        private readonly object _getDirectoryLock = new object();
        protected Directory TdIndexDirectory
        {
            get
            {
                lock (_getDirectoryLock)
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
            
        }
        private readonly object _getAnalyzerLock = new object();
        protected Analyzer TdIndexAnalyzer
        {

            get
            {
                lock (_getAnalyzerLock)
                {
                    return _tdIndexAnalyzer ?? (_tdIndexAnalyzer = new StandardAnalyzer(Version.LUCENE_30));
                }
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

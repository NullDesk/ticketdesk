// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Repositories;
using TicketDesk.Domain.Utilities.Pagination;
using TicketDesk.Domain.Models;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;

namespace TicketDesk.Domain.Services
{
    [Export]
    public class TicketSearchService
    {

        [ImportingConstructor]
        public TicketSearchService([Import("LuceneDirectory")] string indexLocation)
        {
            IndexLocation = indexLocation;
        }

        private string IndexLocation { get; set; }
        private Directory _tdSearchDirectory;
        private bool isRamDirectory = false;

        private Directory TdSearchDirectory
        {
            get
            {
                if (_tdSearchDirectory == null)
                {
                    if (string.Equals(IndexLocation, "ram", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _tdSearchDirectory = new Lucene.Net.Store.RAMDirectory();
                        isRamDirectory = true;
                    }
                    else
                    {
                        var dirInfo = new System.IO.DirectoryInfo(IndexLocation);
                        _tdSearchDirectory = FSDirectory.Open(dirInfo);
                    }
                }

                return _tdSearchDirectory;
            }

        }

        private static object resetDirectoryLock = new object();
        private void ResetTdSearchDirectory()
        {
            lock (resetDirectoryLock)
            {
                if (!isRamDirectory && _tdSearchDirectory != null)
                {
                    _tdSearchDirectory.Close();
                    _tdSearchDirectory = null;
                }
            }
        }

        private Analyzer _tdIndexAnalyzer;
        private Analyzer TdIndexAnalyzer
        {
            get
            {
                if (_tdIndexAnalyzer == null)
                {
                    _tdIndexAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
                }
                return _tdIndexAnalyzer;
            }
        }

        private Searcher _tdIndexSearcher;
        private Searcher TdIndexSearcher
        {
            get
            {
                if (_tdIndexSearcher == null)
                {
                    _tdIndexSearcher = new IndexSearcher(TdSearchDirectory, true);
                }
                return _tdIndexSearcher;
            }
        }
        private static object resetSearcherLock = new object();
        private void ResetTdIndexSearcher()
        {
            lock (resetSearcherLock)
            {
                if (_tdIndexSearcher != null)
                {
                    _tdIndexSearcher.Close();
                    _tdIndexSearcher = null;
                }
            }
        }

        public void InitializeSearch(ITicketService ticketService)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => BuildIndex(ticketService));
        }


        public void UpdateIndex(IEnumerable<Ticket> tickets)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => UpdateTicket(tickets));
        }

        private static object updateLock = new object();
        public void UpdateTicket(IEnumerable<Ticket> tickets)
        {
            lock (updateLock)
            {
                IndexWriter writer = new IndexWriter(TdSearchDirectory, TdIndexAnalyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
                foreach (var ticket in tickets)
                {
                    writer.DeleteDocuments(new Term("ticketid", ticket.TicketId.ToString()));
                    var doc = CreateIndexDocuementForTicket(ticket);
                    writer.AddDocument(doc);
                }
                writer.Optimize();
                writer.Close();
                ResetTdSearchDirectory();
                ResetTdIndexSearcher();

            }
        }


 

        private object buildLock = new object();
        private void BuildIndex(ITicketService ticketService)
        {
            lock (buildLock)
            {
                //index writer will open existing index and merge changes with it
                IndexWriter writer = new IndexWriter(TdSearchDirectory, TdIndexAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED);
                writer.SetMergeFactor(25);
                writer.DeleteAll();
                //process tickets in batches of 25
                IPagination<Ticket> tickets = null;
                var p = 1;
                do
                {
                    tickets = ticketService.ListTickets(p, 25, true);

                    foreach (var ticket in tickets)
                    {
                        var doc = CreateIndexDocuementForTicket(ticket);//make the doc
                        
                        //writer.DeleteDocuments(new Term("ticketid", ticket.TicketId.ToString()));//delete any existing references in the index
                        //write the document to (or back to) the index
                        writer.AddDocument(doc);
                    }
                    p++;
                    tickets = (tickets.HasNextPage) ? ticketService.ListTickets(p, 25, true) : null;
                   
                } while (tickets != null);
               
                //optimize and close the writer
                writer.Commit();
                writer.Optimize();
                writer.Close();

                //close the shared instacnes of the directory and searcher so new searches grab new instances.
                ResetTdSearchDirectory();
                ResetTdIndexSearcher();
            }
        }


        public IEnumerable<Ticket> SearchIndex(ITicketService ticketService, string searchText, out string queryTerm)
        {
            string[] fields = new[] { "title", "details", "tags", "comments" };
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, TdIndexAnalyzer);

            Query query = parser.Parse(searchText);

            queryTerm = query.ToString();
            TopScoreDocCollector collector = TopScoreDocCollector.create(20, true);

            TdIndexSearcher.Search(query, collector);

            ScoreDoc[] hits = collector.TopDocs().scoreDocs;

            SortedList<int, int> ticketIDs = new SortedList<int, int>();
            var o = 0;

            foreach (ScoreDoc scoreDoc in hits)
            {
                //Get the document that represents the search result.
                Document document = TdIndexSearcher.Doc(scoreDoc.doc);

                int ticketID = int.Parse(document.Get("ticketid"));

                //The same document can be returned multiple times within the search results.
                if (!ticketIDs.Values.Contains(ticketID))
                {
                    ticketIDs.Add(o, ticketID);
                    o++;

                }
            }
            return ticketService.ListTickets(ticketIDs, false);

        }

        private Document CreateIndexDocuementForTicket(Ticket ticket)
        {
            var doc = new Document();
            
            var commentTexts = (from c in ticket.TicketComments
                                select c.Comment);
            StringBuilder sb = new StringBuilder();
            foreach (var c in commentTexts)
            {
                sb.AppendLine(c);
            }
            var commentText = sb.ToString();

            Lucene.Net.Documents.Field idField = new Lucene.Net.Documents.Field
                                                   (
                                                       "ticketid",
                                                       ticket.TicketId.ToString(),
                                                       Lucene.Net.Documents.Field.Store.YES,
                                                       Lucene.Net.Documents.Field.Index.NO,
                                                       Lucene.Net.Documents.Field.TermVector.NO
                                                   );

            Lucene.Net.Documents.Field titleField = new Lucene.Net.Documents.Field
                                                    (
                                                        "title",
                                                        ticket.Title ?? string.Empty,
                                                        Lucene.Net.Documents.Field.Store.YES,
                                                        Lucene.Net.Documents.Field.Index.ANALYZED,
                                                        Lucene.Net.Documents.Field.TermVector.YES
                                                    );
            titleField.SetBoost(1.5F);

            Lucene.Net.Documents.Field detailsField = new Lucene.Net.Documents.Field
                                                    (
                                                        "details",
                                                        ticket.Details ?? string.Empty,
                                                        Lucene.Net.Documents.Field.Store.NO,
                                                        Lucene.Net.Documents.Field.Index.ANALYZED,
                                                        Lucene.Net.Documents.Field.TermVector.YES
                                                    );
            detailsField.SetBoost(1F);



            Lucene.Net.Documents.Field tagsField = new Lucene.Net.Documents.Field
                                                    (
                                                        "tags",
                                                        ticket.TagList ?? string.Empty,
                                                        Lucene.Net.Documents.Field.Store.NO,
                                                        Lucene.Net.Documents.Field.Index.ANALYZED,
                                                        Lucene.Net.Documents.Field.TermVector.NO
                                                    );
            tagsField.SetBoost(2F);

            Lucene.Net.Documents.Field commentsField = new Lucene.Net.Documents.Field
                                                    (
                                                        "comments",
                                                        commentText ?? string.Empty,
                                                        Lucene.Net.Documents.Field.Store.NO,
                                                        Lucene.Net.Documents.Field.Index.ANALYZED,
                                                        Lucene.Net.Documents.Field.TermVector.YES
                                                    );
            commentsField.SetBoost(.8F);


            doc.Add(idField);
            doc.Add(titleField);
            doc.Add(detailsField);
            doc.Add(tagsField);
            doc.Add(commentsField);
            if (ticket.CurrentStatus != "Closed")
            {
                doc.SetBoost(10F);
            }
            return doc;
        }
    }
}

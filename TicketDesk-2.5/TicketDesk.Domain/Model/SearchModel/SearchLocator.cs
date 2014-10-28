using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Model.Search
{
    public class SearchLocator: SearchIndexManagerBase
    {
        public IndexSearcher TdIndexSearcher { get; set; }

        public SearchLocator(string indexLocation) : base(indexLocation)
        {
            TdIndexSearcher = new IndexSearcher(TdIndexDirectory, true);
        }

        public IEnumerable<Ticket> SearchIndex(IQueryable<Ticket> tickets, string searchText, out string queryTerm)
        {
            var fields = new[] { "title", "details", "tags", "comments" };
            var parser = new MultiFieldQueryParser(Version.LUCENE_30,
                fields,
                TdIndexAnalyzer);

            var query = parser.Parse(searchText);

            queryTerm = query.ToString();
            var collector = TopScoreDocCollector.Create(20, true);

            TdIndexSearcher.Search(query, collector);

            var hits = collector.TopDocs().ScoreDocs;

            var ticketIds = new SortedList<int, int>();
            var o = 0;

            foreach (var scoreDoc in hits)
            {
                //Get the document that represents the search result.
                var document = TdIndexSearcher.Doc(scoreDoc.Doc);

                var ticketId = int.Parse(document.Get("id"));

                //The same document can be returned multiple times within the search results.
                if (!ticketIds.Values.Contains(ticketId))
                {
                    ticketIds.Add(o, ticketId);
                    o++;
                }
            }

            return from i in ticketIds
                      join t in tickets
                      on i.Value equals t.TicketId
                      orderby i.Key
                      select t;

        }
    }
}

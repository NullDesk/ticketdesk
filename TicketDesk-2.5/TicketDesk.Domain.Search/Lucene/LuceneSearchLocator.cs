using System.Collections.Generic;
using System.Linq;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Search.Lucene
{
    public class SearchLocator: LuceneSearchConnector
    {
        public IndexSearcher TdIndexSearcher { get; set; }

        public SearchLocator(string indexLocation) : base(indexLocation)
        {
            TdIndexSearcher = new IndexSearcher(TdIndexDirectory, true);
        }

        public IEnumerable<SearchResultItem> SearchIndex(string searchText, out string queryTerm)
        {
            var fields = new[] {"id", "title", "details", "tags", "comments"};
            var parser = new MultiFieldQueryParser(Version.LUCENE_30,
                fields,
                TdIndexAnalyzer);

            var query = parser.Parse(searchText);

            queryTerm = query.ToString();
            var collector = TopScoreDocCollector.Create(20, true);

            TdIndexSearcher.Search(query, collector);

            return collector.TopDocs().ScoreDocs.Select(d =>
            { 
                var document = TdIndexSearcher.Doc(d.Doc);
                return new SearchResultItem
                {
                    Id = int.Parse(document.Get("id")),
                    SearchScore = d.Score
                };
            });
        

        }
    }
}

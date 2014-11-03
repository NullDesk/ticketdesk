using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Domain.Search.Lucene
{
    internal class LuceneSearchLocator: LuceneSearchConnector, ISearchLocator
    {
        private IndexSearcher TdIndexSearcher { get; set; }

        internal LuceneSearchLocator(string indexLocation)
            : base(indexLocation)
        {
            TdIndexSearcher = new IndexSearcher(TdIndexDirectory, true);
        }

        public Task<IEnumerable<SearchResultItem>> SearchAsync(string searchText)
        {
            return Task.Run(() =>
            {
                var fields = new[] {"id", "title", "details", "tags", "comments"};
                var parser = new MultiFieldQueryParser(Version.LUCENE_30,
                    fields,
                    TdIndexAnalyzer);

                var query = parser.Parse(searchText);
                
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
            });
        }
    }
}

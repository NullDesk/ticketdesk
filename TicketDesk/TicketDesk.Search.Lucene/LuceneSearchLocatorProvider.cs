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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using TicketDesk.Search.Common;
using Version = Lucene.Net.Util.Version;

namespace TicketDesk.Search.Lucene
{
    public class LuceneSearchLocatorProvider: LuceneSearchConnector, ISearchLocatorProvider
    {
        public LuceneSearchLocatorProvider()
            : base("ticketdesk-searchindex") { }

        public Task<IEnumerable<SearchResultItem>> SearchAsync(string searchText, int projectId)
        {
            return Task.Run(() =>
            {
                //new searcher each time, 
                //  because they don't see commits from any writer after being opened.
                //  even in huge deployments though, TD's number of concurrent searches
                //  would be tiny... not worth coding a mechanism to detect updates and
                //  reuse a searcher until the index is updated. In actuality, index
                //  updates would certainly be more frequent than index searches.
                
                var searcher = new IndexSearcher(TdIndexDirectory, true);
                
                var fields = new[] {"id", "title", "details", "tags", "events"};
                var parser = new MultiFieldQueryParser(Version.LUCENE_30,
                    fields,
                    TdIndexAnalyzer);

                var query = parser.Parse(searchText);
                
                
                var collector = TopScoreDocCollector.Create(20, true);
                if (projectId != default(int))
                {
                    var filterQuery = new BooleanQuery();
                    filterQuery.Add(new TermQuery(new Term("projectid", projectId.ToString())), Occur.MUST);
                    var filter = new QueryWrapperFilter(filterQuery);
                    searcher.Search(query, filter, collector);
                }
                else
                {
                    searcher.Search(query, collector);
                }
                return collector.TopDocs().ScoreDocs.Select(d =>
                {
                    var document = searcher.Doc(d.Doc);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedDog.Search.Model;

namespace TicketDesk.Domain.Search.AzureSearch
{
    internal class AzureSearchLocator : AzureSearchConector, ISearchLocator
    {
        private string IndexName { get; set; }

        internal AzureSearchLocator(string indexName)
        {
            IndexName = indexName;
        }

        public async Task<IEnumerable<SearchResultItem>> SearchAsync(string searchText)
        {
            var query = new SearchQuery(searchText)
            {
                SearchFields = "id,title,details,tags,comments",
                Select = "id"
            };
            var result = await QueryClient.SearchAsync(IndexName, query);
            if (result.IsSuccess)
            {
                return
                    result.Body.Records.Select(
                        r =>
                            new SearchResultItem
                            {
                                Id = int.Parse((string) r.Properties["id"]),
                                SearchScore = (float) r.Score
                            });
            }
            return new SearchResultItem[0];
        }
    }
}

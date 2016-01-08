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
using RedDog.Search.Model;
using TicketDesk.Search.Common;

namespace TicketDesk.Search.Azure
{
    public class AzureSearchLocatorProvider : AzureSearchConector, ISearchLocatorProvider
    {
        private const string IndexName = "ticketdesk-searchindex";


        public async Task<IEnumerable<SearchResultItem>> SearchAsync(string searchText, int projectId)
        {
            var query = new SearchQuery(searchText)
            {
                SearchFields = "id,title,details,tags,events",
                Select = "id",
                
            };
            if (projectId != default(int))
            {
                query.Filter = string.Format("projectid eq '{0}'", projectId);
            }
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

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
using TicketDesk.Search.Common;


namespace TicketDesk.Domain.Model
{
    public static class SearchManagerExtensions
    {
        public static async Task<IEnumerable<Ticket>> SearchAsync(this TdSearchContext manager, IQueryable<Ticket> ticketQuery,
            string searchText, int projectId)
        {
            var results = await manager.IndexSearcher.SearchAsync(searchText, projectId);

            var inFilter = results.Select(r => r.Id).ToList();

            return from t in
                       (from t in ticketQuery
                        where inFilter.Contains(t.TicketId)
                        select t).ToList()
                   join i in results
                   on t.TicketId equals i.Id
                   orderby i.SearchScore descending
                   select t;
        } 
    }
}

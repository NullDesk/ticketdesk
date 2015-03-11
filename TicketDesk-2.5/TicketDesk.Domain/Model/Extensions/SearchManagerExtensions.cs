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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketDesk.Search.Common;


namespace TicketDesk.Domain.Model
{
    public static class SearchManagerExtensions
    {
        public static async Task<IEnumerable<Ticket>> SearchAsync(this TicketDeskSearchContext manager, IQueryable<Ticket> ticketQuery,
            string searchText)
        {
            var results = await manager.IndexSearcher.SearchAsync(searchText);
            
            return from i in results
                   join t in ticketQuery
                   on i.Id equals t.TicketId
                   orderby i.SearchScore descending 
                   select t;
        } 
    }
}

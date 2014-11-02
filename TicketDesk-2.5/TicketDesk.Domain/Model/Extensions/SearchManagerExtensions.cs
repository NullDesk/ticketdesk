using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Search;

namespace TicketDesk.Domain.Model
{
    public static class SearchManagerExtensions
    {
        public static async Task<IEnumerable<Ticket>> SearchAsync(this SearchManager manager, IQueryable<Ticket> ticketQuery,
            string searchText)
        {
            var results = await manager.SearchIndexAsync(searchText);
            
            return from i in results
                   join t in ticketQuery
                   on i.Id equals t.TicketId
                   orderby i.SearchScore descending 
                   select t;
        } 
    }
}

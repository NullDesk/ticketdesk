using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Search;

namespace TicketDesk.Domain.Model
{
    public static class TicketExtensions
    {
        public static IEnumerable<SearchQueueItem> ToSeachQueueItems(this IEnumerable<Ticket> tickets)
        {
            return tickets.Select(t => new SearchQueueItem
            {
                Id = t.TicketId,
                Title = t.Title,
                Details = t.Details,
                Status = t.TicketStatus.ToString(),
                LastUpdateDate = t.LastUpdateDate,
                Tags = t.TagList.Split(','),
                Comments = t.TicketComments.Select(c => c.Comment).ToArray()
            });
        }
    }
}

using System.Collections.Generic;
using System.Linq;
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
                //not null comments only, otherwise we end up indexing empty array item, or blowing up azure required field
                Events = t.TicketEvents.Where(c => !string.IsNullOrEmpty(c.Comment)).Select(c => c.Comment).ToArray()
            });
        }

       
    }
}

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

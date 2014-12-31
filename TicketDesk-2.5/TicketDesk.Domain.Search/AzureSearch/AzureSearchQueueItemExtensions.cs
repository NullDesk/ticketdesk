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

using System.Globalization;
using RedDog.Search.Model;

namespace TicketDesk.Domain.Search.AzureSearch
{
    public static class AzureSearchQueueItemExtensions
    {
        public static IndexOperation ToIndexOperation(this SearchQueueItem item)
        {
            var op = new IndexOperation(IndexOperationType.Upload, "id", item.Id.ToString(CultureInfo.InvariantCulture))
                .WithProperty("title", item.Title)
                .WithProperty("status", item.Status)
                .WithProperty("lastupdatedate", item.LastUpdateDate)
                .WithProperty("details", item.Details)
                .WithProperty("tags", item.Tags)
                .WithProperty("events", item.Events);
            return op;
        }
    }
}

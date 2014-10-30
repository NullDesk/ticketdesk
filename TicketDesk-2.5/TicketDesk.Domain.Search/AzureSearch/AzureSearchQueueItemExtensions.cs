using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedDog.Search.Model;

namespace TicketDesk.Domain.Search.AzureSearch
{
    public static class AzureSearchQueueItemExtensions
    {
        public static IndexOperation ToIndexOperation(this SearchQueueItem item)
        {
            return new IndexOperation(IndexOperationType.Upload, "id", item.Id.ToString(CultureInfo.InvariantCulture))
                .WithProperty("title", item.Title)
                .WithProperty("status", item.Status)
                .WithProperty("lastupdatedate", item.LastUpdateDate)
                .WithProperty("details", item.Details)
                .WithProperty("tags", item.Tags)
                .WithProperty("comments", item.Comments);
        }
    }
}

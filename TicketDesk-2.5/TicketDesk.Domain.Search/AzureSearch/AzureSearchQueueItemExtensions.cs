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
                .WithProperty("comments", item.Comments);
            return op;
        }
    }
}

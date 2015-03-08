using System;
using Microsoft.Azure.WebJobs;
using TicketDesk.Domain.Search;

namespace TicketDesk.Background.Job
{
    public class SearchIndexerJob
    {
        public static void IndexDocument([QueueTrigger("ticket-search-queue")] SearchQueueItem document)
        {
            Console.Out.WriteLine("Indexing triggered for ticket #" + document.Id);
            var task = Program.SearchProvider.AddItemsToIndexAsync(new[] { document });
            task.Wait();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TicketDesk.Domain.Search;

namespace TicketDesk.Background.Job
{
    public class SearchIndexerJob
    {
        public static void IndexDocument([QueueTrigger("ticket-search-queue")] SearchQueueItem document)
        {
            var task = Program.SearchProvider.AddItemsToIndexAsync(new[] { document });
            task.Wait();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TicketDesk.Search.Azure;
using TicketDesk.Search.Common;

namespace TicketDesk.Background.Job
{
    public class SearchIndexerJob
    {
        public static void IndexDocument([QueueTrigger("ticket-search-queue")] SearchIndexItem document)
        {
            Console.Out.WriteLine("Indexing triggered for ticket #" + document.Id);
            var task = Program.SearchProvider.SendItemsToIndexAsync(new[] { document });
            task.Wait();
        }
    }
}

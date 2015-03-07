using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Search;
using TicketDesk.IO;

namespace TicketDesk.Web.Client
{
    public static class SearchInitializer
    {
        public static void ConfigureSearch()
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            HostingEnvironment.QueueBackgroundWorkItem(async ct =>
            {
                //TODO: decide if we want to rebuild lucene on start or not, currently doesn't rebuild automatically
                await context.SearchManager.InitializeSearchAsync();
            });

            if (context.SearchManager.SearchQueue is MemoryQueueProvider)
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => MonitorQueue(ct));
            }
        }

        /// <summary>
        /// Monitors the in-memory search queue and processes pending search items foud there.
        /// </summary>
        /// <remarks>
        /// This should be run on a background thread, and only if the search queue is an in-memory implementation.
        /// </remarks>
        /// <param name="ct"></param>
        private static async void MonitorQueue(CancellationToken ct)
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            while (!ct.IsCancellationRequested)
            {
                var searchQueueItems = GetAllSearchQueueItems(context);
                if (searchQueueItems.Any())
                {
                    await context.SearchManager.AddItemsToIndexAsync(searchQueueItems);
                }
                else
                {
                    await Task.Delay(5000, ct); //queue returned nothing, wait 5 seconds
                }
            }
            //cancellation requested, flush the queue now and hope it finishes within 90 seconds (which is certainly should)
            await context.SearchManager.AddItemsToIndexAsync(GetAllSearchQueueItems(context));
        }

        private static SearchQueueItem[] GetAllSearchQueueItems(TicketDeskContext context)
        {
            var items = context.SearchManager.SearchQueue.DequeueAllItems<SearchQueueItem>();
            var searchQueueItems = items as SearchQueueItem[] ?? items.ToArray();
            return searchQueueItems;
        }
    }
}
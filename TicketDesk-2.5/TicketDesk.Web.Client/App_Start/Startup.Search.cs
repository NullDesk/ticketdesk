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

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using TicketDesk.Domain.Search;
using TicketDesk.IO;

namespace TicketDesk.Web.Client
{
	public partial class Startup
	{
        public void ConfigureSearch()
        {

            HostingEnvironment.QueueBackgroundWorkItem(async ct =>
            {
                //TODO: decide if we want to rebuild lucene on start or not, 
                //  currently doesn't rebuild automatically.
                //  does call optimize on start
                await TicketDeskSearchManager.Current.InitializeSearchAsync();
            });

            HostingEnvironment.QueueBackgroundWorkItem(ct => PerformIndexMaintenance(ct));

            if (TicketDeskSearchManager.Current.SearchQueue is MemoryQueueProvider)
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => MonitorQueue(ct));
            }
        }

        private async void PerformIndexMaintenance(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(900000, ct);//wait 15 minutes
                await TicketDeskSearchManager.Current.RunIndexMaintenanceAsync();
            }
        }

        /// <summary>
        /// Monitors the in-memory search queue and processes pending search items foud there.
        /// </summary>
        /// <remarks>
        /// This should be run on a background thread, and only if the search queue is an in-memory implementation.
        /// </remarks>
        /// <param name="ct"></param>
        private async void MonitorQueue(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(5000, ct); //wait 5 seconds between checks
                var searchQueueItems = GetAllSearchQueueItems();
                if (searchQueueItems.Any())
                {
                    await TicketDeskSearchManager.Current.AddItemsToIndexAsync(searchQueueItems);
                }

            }
            //cancellation requested, flush the queue now and hope it finishes within 90 seconds (which is certainly should)
            await TicketDeskSearchManager.Current.AddItemsToIndexAsync(GetAllSearchQueueItems());
        }

        private SearchQueueItem[] GetAllSearchQueueItems()
        {
            var items = TicketDeskSearchManager.Current.SearchQueue.DequeueAllItems<SearchQueueItem>();
            var searchQueueItems = items as SearchQueueItem[] ?? items.ToArray();
            return searchQueueItems;
        }
	}
}
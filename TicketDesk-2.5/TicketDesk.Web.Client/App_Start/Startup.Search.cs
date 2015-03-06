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

using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using TicketDesk.Domain;
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
                var context = DependencyResolver.Current.GetService<TicketDeskContext>();
                await context.SearchProvider.InitializeSearchAsync();
            });


            if (TicketDeskSearchProvider.SearchQueue is MemoryQueueProvider)
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
                var item = await TicketDeskSearchProvider.SearchQueue.DequeueItemAsync<SearchQueueItem>();
                if (item == null)
                {
                    await Task.Delay(5000, ct);//queue returned nothing, wait 5 seconds
                }
                else
                {
                    await context.SearchProvider.AddItemsToIndexAsync(new[] { item });
                }
            }
        }
    }
}
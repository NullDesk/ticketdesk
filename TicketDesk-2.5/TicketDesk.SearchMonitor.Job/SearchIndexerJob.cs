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

using Microsoft.Azure.WebJobs;
using TicketDesk.Search.Common;

namespace TicketDesk.SearchMonitor.Job
{
    public class SearchMonitor
    {
        public static void IndexDocument([QueueTrigger("ticket-search-queue")] SearchIndexItem document)
        {
            var task = Program.SearchProvider.SendItemsToIndexAsync(new[] { document });
            task.Wait();
        }
    }
}

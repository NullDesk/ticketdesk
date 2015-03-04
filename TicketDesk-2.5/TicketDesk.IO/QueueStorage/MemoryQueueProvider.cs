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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketDesk.IO
{
    /// <summary>
    /// Provides services for adding simple string messages to an in-memory queue
    /// </summary>
    public class MemoryQueueProvider: IQueueProvider
    {
        private static Queue<string> _queue;

        private static Queue<string> Queue
        {
            get { return _queue ?? (_queue = new Queue<string>()); }
        }

        public async Task QueueItemsAsync(IEnumerable<object> items)
        {
            foreach (var item in items.Select(JsonConvert.SerializeObject))
            {
                Queue.Enqueue(item);
            }
            await Task.FromResult<object>(null);
        }
    }
}

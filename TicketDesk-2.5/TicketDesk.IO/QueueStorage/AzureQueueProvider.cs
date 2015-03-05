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
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace TicketDesk.IO
{
    /// <summary>
    /// Provides services for adding simple messages to an azure storage queue
    /// </summary>
    public class AzureQueueProvider : IQueueProvider
    {
        private CloudQueue Queue { get; set; }
        public AzureQueueProvider(CloudQueue queue)
        {
            Queue = queue;
        }

        public async Task EnqueueItemsAsync(IEnumerable<object> items)
        {
            var tasks = items.Select(QueueItemAsyc).ToList();
            await Task.WhenAll(tasks);
        }

        private async Task QueueItemAsyc(object item)
        {
            var jItem = JsonConvert.SerializeObject(item);
            var message = new CloudQueueMessage(jItem);
            await Queue.AddMessageAsync(message);
        }

        public async Task<T> DequeueItem<T>() where T: class
        {
            var message = await Queue.GetMessageAsync();
            return JsonConvert.DeserializeObject<T>(message.AsString);
        }
    }
}

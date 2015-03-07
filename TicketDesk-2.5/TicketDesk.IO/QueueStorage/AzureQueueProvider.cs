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

using System;
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

        public async Task<T> DequeueItemAsync<T>() where T: class
        {
            var message = await Queue.GetMessageAsync();
            if (message == null)
            {
                return null;
            }
            Queue.DeleteMessage(message);
            return JsonConvert.DeserializeObject<T>(message.AsString);
        }


        public IEnumerable<T> DequeueAllItems<T>() where T : class
        {
            var messages = new List<T>();
            var currentCount = 0;
            var lastCount = -1;
            while (lastCount < currentCount)
            {
                lastCount = messages.Count;
                messages.AddRange(GetQueueMessageBatch<T>());
                currentCount = messages.Count;
            }
            return messages;
        }

        private IEnumerable<T> GetQueueMessageBatch<T>() where T : class
        {
            IEnumerable<T> result = null;
            var messages = Queue.GetMessages(32);
            var cloudQueueMessages = messages as CloudQueueMessage[] ?? messages.ToArray();
            if (cloudQueueMessages.Any())
            {
                result = cloudQueueMessages.Select(m => JsonConvert.DeserializeObject<T>(m.AsString));
                foreach (var message in cloudQueueMessages)
                {
                    Queue.DeleteMessage(message);
                }
            }
            return result;
        }
    }
}

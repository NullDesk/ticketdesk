using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AzureBlobFileSystem;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace TicketDesk.IO
{
    public interface IQueueProvider
    {
        Task QueueItemsAsync(IEnumerable<object> items);
    }

    public class AzureQueueProvider : IQueueProvider
    {
        private CloudQueue Queue { get; set; }
        public AzureQueueProvider(CloudQueue queue)
        {
            Queue = queue;
        }

        public async Task QueueItemsAsync(IEnumerable<object> items)
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
    }

    public class DatabaseQueueProvider : IQueueProvider
    {
        public Task QueueItemsAsync(IEnumerable<object> items)
        {
            throw new NotImplementedException();
        }
    }

    public class TicketDeskQueueStorage
    {
        private static IQueueProvider _currentSearchQueueProvider;
        public static IQueueProvider CurrentSearchQueue
        {
            get
            {
                if (_currentSearchQueueProvider == null)
                {
                    var account = AzureConnectionHelper.CloudStorageAccount;
                    if (account != null)
                    {
                        var queueClient = account.CreateCloudQueueClient();
                        var searchQueue = queueClient.GetQueueReference("ticketdesk-search-queue");
                        var qt = searchQueue.CreateIfNotExistsAsync();
                        qt.Wait();
                        _currentSearchQueueProvider = new AzureQueueProvider(searchQueue);
                    }
                    else
                    {
                        //make a db queue instead
                    }
                }
                return _currentSearchQueueProvider;
            }
        }

       
    }
}

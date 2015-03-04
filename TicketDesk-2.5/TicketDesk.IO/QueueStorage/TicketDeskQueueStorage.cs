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

namespace TicketDesk.IO
{
    public static class TicketDeskQueueStorage
    {
        private static Dictionary<string, IQueueProvider> _queues;

        private static Dictionary<string, IQueueProvider> Queues
        {
            get { return _queues ?? (_queues = new Dictionary<string, IQueueProvider>()); }
        }


        public static IQueueProvider GetQueue(string queueName)
        {
            if (!Queues.ContainsKey(queueName))
            {
                Queues.Add(queueName, CreateQueue(queueName));
            }
            return Queues[queueName];
        }

        private static IQueueProvider CreateQueue(string queueName)
        {
            IQueueProvider q;
            var account = AzureConnectionHelper.CloudStorageAccount;
            if (account != null)
            {
                var queueClient = account.CreateCloudQueueClient();
                var searchQueue = queueClient.GetQueueReference(queueName);
                var qt = searchQueue.CreateIfNotExistsAsync();
                qt.Wait();
                q = new AzureQueueProvider(searchQueue);
            }
            else
            {
                q = new MemoryQueueProvider();
            }

            return q;
        }

    }
}

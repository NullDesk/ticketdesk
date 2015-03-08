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
using Microsoft.Azure.WebJobs;
using TicketDesk.Domain;
using TicketDesk.Domain.Search;
using TicketDesk.IO;

namespace TicketDesk.Background.Job
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        internal static TicketDeskSearchManager SearchProvider;

        static void Main()
        {
            Initialize();
            var storageConnectionString = AzureConnectionHelper.CloudConfigConnString ?? AzureConnectionHelper.ConfigManagerConnString;
            var host = new JobHost(new JobHostConfiguration(storageConnectionString));
            host.RunAndBlock();
        }

        private static void Initialize()
        {
            
            SearchProvider = TicketDeskSearchManager.Current;
            Console.Out.WriteLine("SearchProvider Connector Type: " + SearchProvider.GetConnectorType());
            Console.Out.WriteLine("Search Queue Type: " + SearchProvider.SearchQueue.GetType());
        }
    }
}

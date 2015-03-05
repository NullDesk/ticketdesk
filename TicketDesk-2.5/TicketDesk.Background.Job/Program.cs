using System;
using System.Data.Entity;
using Microsoft.Azure.WebJobs;
using TicketDesk.Domain;
using TicketDesk.Domain.Migrations;
using TicketDesk.Domain.Search;
using TicketDesk.IO;

namespace TicketDesk.Background.Job
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        internal static TicketDeskSearchProvider SearchProvider;

        static void Main()
        {
            Initialize();
            var storageConnectionString = AzureConnectionHelper.CloudConfigConnString ?? AzureConnectionHelper.ConfigManagerConnString;
            var host = new JobHost(new JobHostConfiguration(storageConnectionString));
            host.RunAndBlock();
        }

        private static void Initialize()
        {
            var context = new TicketDeskContext();
            SearchProvider = context.SearchProvider;
        }
    }
}

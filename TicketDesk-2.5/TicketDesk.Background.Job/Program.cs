using Microsoft.Azure.WebJobs;
using TicketDesk.Domain;
using TicketDesk.Domain.Search;

namespace TicketDesk.Background.Job
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        internal static TicketDeskSearchProvider SearchProvider;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Initialize();
            var host = new JobHost();
            host.RunAndBlock();
        }

        private static void Initialize()
        {
            var context = new TicketDeskContext();
            SearchProvider = context.SearchProvider; 
        }
    }
}

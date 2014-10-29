using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Search;

namespace TicketDesk.Diagnostics.AzureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var mgr = new IndexManager();
            var qClient = mgr.QueryClient;
            Console.WriteLine(qClient.ToString());
            var t = mgr.IndexExists();
            t.Wait();

            Console.WriteLine("Index " + (t.Result ? "Exists" : "Not Found"));
            //mgr.RemoveTicketIndex();
            mgr.CreateTicketIndex();
            Console.ReadLine();
        }
    }
}

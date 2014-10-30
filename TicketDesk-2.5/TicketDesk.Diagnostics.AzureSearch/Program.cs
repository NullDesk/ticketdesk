using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Search;
using TicketDesk.Domain.Search.AzureSearch;

namespace TicketDesk.Diagnostics.AzureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            //var mgr = new AzureIndexManager();
            //var e = mgr.IndexExists();
            //e.Wait();
            //if (e.Result)
            //{
            //    var r = mgr.RemoveIndex();
            //    r.Wait();
            //}
            //var t = mgr.CreateIndex();
            //t.Wait();
            //Console.WriteLine("Index Creation " + (t.Result ? "Sucess!":"Failed!"));
            ////var d = new SearchQueueItem(){
            ////    Id = 1, 
            ////    Details = "details here", 
            ////    Comments = new []{"comment one", "comment two"},
            ////    LastUpdateDate = DateTimeOffset.Now,
            ////    Status = "open",
            ////    Tags = new []{"tag1", "tag2", "toast"},
            ////    Title = "nothing to see here"
            ////};
            ////var a = mgr.AddItemToIndex(d.ToIndexOperation());
            ////a.Wait();
            ////Console.WriteLine("Document Creation " + (a.Result ? "Sucess!" : "Failed!"));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}

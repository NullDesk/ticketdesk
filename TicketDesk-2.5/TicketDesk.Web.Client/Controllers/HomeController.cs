using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Search;

namespace TicketDesk.Web.Client.Controllers
{
    public class HomeController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public HomeController(TicketDeskContext context)
        {
            Context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //public async Task<ActionResult> Indexer()
        //{


        //    await Searcher.GenerateIndexAsync();
        //    return View("Index");
        //}
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Domain.Search;

namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    public class SearchManagerController : Controller
    {
        
        private TicketDeskContext Context { get; set; }
        public SearchManagerController(TicketDeskContext context)
        {
          
            Context = context;
        }

        // GET: Admin/SearchManager
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> RemoveIndex()
        {
            await Context.SearchProvider.RemoveIndexAsync();
            ViewBag.IndexRemoved = true;
            return View("Index");
        }
       
        public async Task<ActionResult> PopulateIndex()
        {
            await Context.SearchProvider.InitializeSearch();
            var queueItems = Context.Tickets.Include("TicketEvents").ToSeachQueueItems();
            await Context.SearchProvider.QueueItemsForIndexingAsync(queueItems);
            ViewBag.IndexPopulated = true;
            return View("Index");
        }
    }
}
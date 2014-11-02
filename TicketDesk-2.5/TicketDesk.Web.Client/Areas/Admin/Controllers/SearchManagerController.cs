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
        private SearchManager Search { get; set; }
        private TicketDeskContext Context { get; set; }
        public SearchManagerController(SearchManager search, TicketDeskContext context)
        {
            Search = search;
            Context = context;
        }

        // GET: Admin/SearchManager
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> RemoveIndex()
        {
            await Search.RemoveIndexAsync();
            ViewBag.IndexRemoved = true;
            return View("Index");
        }
        public async Task<ActionResult> CreateIndex()
        {
            await Search.InitializeSearch();
            ViewBag.IndexCreated = true;
            return View("Index");
        }
        public async Task<ActionResult> PopulateIndex()
        {
            var queueItems = Context.Tickets.Include("TicketComments").ToSeachQueueItems();
            await Search.QueueItemsForIndexingAsync(queueItems);
            ViewBag.IndexPopulated = true;
            return View("Index");
        }
    }
}
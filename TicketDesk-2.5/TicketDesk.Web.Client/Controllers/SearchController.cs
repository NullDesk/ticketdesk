using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("Search")]
    public class SearchController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public SearchController(TicketDeskContext context)
        {
            Context = context;
        }

        // GET: Search
        [Route(Name = "SearchResults")]
        public ActionResult Index(string find)
        {
            if (!string.IsNullOrEmpty(find))
            {
                var model = Context.SearchManager.Search(Context.Tickets, find);
                return View(model);
            }
            else
            {
                return View(new Ticket[0]);
            }
           
        }
    }
}
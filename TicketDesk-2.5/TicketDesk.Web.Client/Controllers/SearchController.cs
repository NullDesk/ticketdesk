using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public SearchController(TicketDeskContext context)
        {
            Context = context;
        }

        // GET: Search
       
        public async Task<ActionResult> Index(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var model = await Context.SearchProvider.SearchAsync(Context.Tickets, term);
                return View(model);
            }
            else
            {
                return View(new Ticket[0]);
            }
           
        }
    }
}
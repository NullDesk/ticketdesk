using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    public class SearchController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public SearchController(TicketDeskContext context)
        {
            Context = context;
        }

        // GET: Search
       
        public async Task<ActionResult> Index(string find)
        {
            if (!string.IsNullOrEmpty(find))
            {
                var model = await Context.SearchManager.SearchAsync(Context.Tickets, find);
                return View(model);
            }
            else
            {
                return View(new Ticket[0]);
            }
           
        }
    }
}
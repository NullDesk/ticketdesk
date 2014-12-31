using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("search")]
    [Route("{action=index")]
    [Authorize]
    public class SearchController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public SearchController(TicketDeskContext context)
        {
            Context = context;
        }

        [Route("")]
        [Route("{term}")]
        [HttpGet]
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
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Controllers
{
    /// <summary>
    /// Class TicketController.
    /// </summary>
    [RoutePrefix("ticket")]
    [Authorize]
    public class TicketController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public TicketController(TicketDeskContext context)
        {
            Context = context;
        }

        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Index", "TicketCenter", new { area = "" });
        }

        [Route("{id:int}")]
        public async Task<ActionResult> Index(int id)
        {
            var model = await Context.Tickets.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public async Task<ActionResult> New(Ticket ticket, Guid tempId)
        {
            
            if (ModelState.IsValid)
            {
                var vm = new TicketCreateViewModel(ticket, Context){TempId = tempId};
                
                try
                {
                    if (await vm.CreateTicketAsync())
                    {
                        return RedirectToAction("Index", new { id = ticket.TicketId });
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch// (DbEntityValidationException ex)
                {
                   //TODO: catch rule exceptions? or can annotations handle this fully now?
                }
                
            }
            return View(new TicketCreateViewModel(ticket, Context));
        }


        public async Task<ActionResult> TicketEvents(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            return PartialView("_TicketEvents", ticket.TicketEvents);
        }

        public async Task<ActionResult> TicketDetails(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            return PartialView("_TicketDetails", ticket);
        }


        [Authorize(Roles = "TdInternalUsers")]
        public ActionResult New()
        {
            var model = new TicketCreateViewModel(new Ticket(), Context);
            return View(model);
        }
    }
}
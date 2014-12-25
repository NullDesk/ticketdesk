using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
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

        [Authorize(Roles = "TdInternalUsers")]
        public ActionResult New()
        {
            var model = new Ticket { Owner = Context.SecurityProvider.CurrentUserId };
            ViewBag.TempId = Guid.NewGuid();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public async Task<ActionResult> New(Ticket ticket, Guid tempId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await CreateTicketAsync(ticket, tempId))
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
            ViewBag.TempId = tempId;
            return View(ticket);
        }

        private async Task<bool> CreateTicketAsync(Ticket ticket, Guid tempId)
        {
            Context.Tickets.Add(ticket);
            await Context.SaveChangesAsync();
            ticket.CommitPendingAttachments(tempId);

            return ticket.TicketId != default(int);
        }

        public async Task<ActionResult> TicketEvents(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            return PartialView("_TicketEvents", ticket.TicketEvents);
        }

        public async Task<ActionResult> TicketDetails(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);

            ViewBag.Attachments = TicketDeskFileStore.ListAttachmentInfo(ticketId.ToString(CultureInfo.InvariantCulture), false);

            return PartialView("_TicketDetails", ticket);
        }



    }
}
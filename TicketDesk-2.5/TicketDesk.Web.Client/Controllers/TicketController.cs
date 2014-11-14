using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Controllers
{
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
            var model = new TicketCreateViewModel(new Ticket(), Context);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        public async Task<ActionResult> New(Ticket ticket, Guid tempId)
        {
            if (ModelState.IsValid)
            {
                var vm = new TicketCreateViewModel(ticket, Context);
                
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
    }
}
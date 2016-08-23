// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization.Controllers;

namespace TicketDesk.Web.Client.Controllers
{
    /// <summary>
    /// Class TicketController.
    /// </summary>
    [RoutePrefix("ticket")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    public class TicketController : Controller
    {
        private TdDomainContext Context { get; set; }
        public TicketController(TdDomainContext context)
        {
            Context = context;
        }

        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Index", "TicketCenter");
        }

        [Route("{id:int}")]
        public async Task<ActionResult> Index(int id)
        {
           
            var model = await Context.Tickets.Include(t => t.TicketSubscribers).FirstOrDefaultAsync(t => t.TicketId == id);
            if (model == null)
            {
                return RedirectToAction("Index", "TicketCenter");
            }
            ViewBag.IsEditorDefaultHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote";

            return View(model);
        }

        [Route("new")]
        public async Task<ActionResult> New()
        {

            var model = new Ticket
            {
                Owner = Context.SecurityProvider.CurrentUserId,
                IsHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote"
            };

            await SetProjectInfoForModelAsync(model);

            ViewBag.TempId = Guid.NewGuid();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateOnlyIncomingValues]
        [ValidateInput(false)]
        [Route("new")]
        public async Task<ActionResult> New(Ticket ticket, Guid tempId)
        {
            if (ticket.IsHtml)
            {
                ticket.Details = ticket.Details.StripHtmlWhenEmpty();
                if (string.IsNullOrEmpty(ticket.Details))
                {
                    ModelState.AddModelError("Details", Strings.RequiredField);
                }
            }

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
                catch (DbEntityValidationException)
                {

                    //TODO: catch rule exceptions? or can annotations handle this fully now?
                }

            }
            ViewBag.TempId = tempId;
            await SetProjectInfoForModelAsync(ticket);
            return View(ticket);
        }

        [Route("ticket-files")]
        public ActionResult TicketFiles(int ticketId)
        {
            //WARNING! This is also used as a child action and cannot be made async in MVC 5
            var attachments = TicketDeskFileStore.ListAttachmentInfo(ticketId.ToString(CultureInfo.InvariantCulture), false);
            ViewBag.TicketId = ticketId;
            return PartialView("_TicketFiles", attachments);
        }

        [Route("ticket-events")]
        public ActionResult TicketEvents(int ticketId)
        {
            //WARNING! This is also used as a child action and cannot be made async in MVC 5
            var ticket = Context.Tickets.Find(ticketId);
            return PartialView("_TicketEvents", ticket.TicketEvents);
        }

        [Route("ticket-details")]
        public ActionResult TicketDetails(int ticketId)
        {
            //WARNING! This is also used as a child action and cannot be made async in MVC 5
            var ticket = Context.Tickets.Find(ticketId);
            ViewBag.DisplayProjects = Context.Projects.Any();

            return PartialView("_TicketDetails", ticket);
        }

        [Route("change-ticket-subscription")]
        [HttpPost]
        public async Task<JsonResult> ChangeTicketSubscription(int ticketId)
        {
            var userId = Context.SecurityProvider.CurrentUserId;
            var ticket = await Context.Tickets.Include(t => t.TicketSubscribers).Include(t => t.TicketEvents.Select(e => e.TicketEventNotifications)).FirstOrDefaultAsync(t => t.TicketId == ticketId);
            var subscriber =
                ticket.TicketSubscribers.FirstOrDefault(s => s.SubscriberId == Context.SecurityProvider.CurrentUserId);
            var isSubscribed = false;
            if (subscriber == null)
            {
                subscriber = new TicketSubscriber
                {
                    SubscriberId = userId,
                };
                ticket.TicketSubscribers.Add(subscriber);
                isSubscribed = true;
            }
            else
            {
                ticket.TicketSubscribers.Remove(subscriber);
            }
            await Context.SaveChangesAsync();
            return new JsonCamelCaseResult { Data = new { IsSubscribed = isSubscribed } };
        }

        private async Task SetProjectInfoForModelAsync(Ticket ticket)
        {
            if (ticket.ProjectId == default(int))
            {
                var projects = await Context.Projects.Select(s=>s.ProjectId).ToListAsync();
                var isMulti = (projects.Count > 1);
                ViewBag.IsMultiProject = isMulti;
               
                //set to first project if only one project exists, otherwise use user's selected project
                ticket.ProjectId = (isMulti) ? await Context.UserSettingsManager.GetUserSelectedProjectIdAsync(Context) : projects.FirstOrDefault(); 
            }
        }


        private async Task<bool> CreateTicketAsync(Ticket ticket, Guid tempId)
        {

            Context.Tickets.Add(ticket);
            await Context.SaveChangesAsync();
            ticket.CommitPendingAttachments(tempId);

            return ticket.TicketId != default(int);
        }

    }
}
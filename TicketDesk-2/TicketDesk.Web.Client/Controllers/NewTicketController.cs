// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Models;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Client.Helpers;
using System.ComponentModel.Composition;
namespace TicketDesk.Web.Client.Controllers
{
    [ValidateInput(false)]
    [Export("NewTicket", typeof(IController))]
    public partial class NewTicketController : ApplicationController
    {
        private ITicketService Tickets { get; set; }
        private SettingsService Settings { get; set; }
        
        [ImportingConstructor]
        public NewTicketController(ITicketService ticketService, ISecurityService security, SettingsService settings)
            : base(security)
        {
            Tickets = ticketService;
            Settings = settings;
        }

        [Authorize]
        [ValidateInput(false)]
        public virtual ActionResult Create()
        {
            Ticket ticket = new Ticket();
            var model = new TicketCreateViewModel(Security, Settings, ticket);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult Create
            (
                [ModelBinder(typeof(TicketWithAttachmentsModelBinder))]
                [Bind(Prefix = "Ticket")] 
                Ticket ticket
            )
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(ticket.Owner))
                {
                    //TODO: This smells like business logic to me, see if it can be moved into the domain entities or view model
                    ticket.Owner = Security.CurrentUserName;
                }

                int? newTicketId = null;
                try
                {
                    //newTicketId = 1;
                    newTicketId = Tickets.CreateNewTicket(ticket);
                    //TODO: When create fails, the pending attachments are lost. Need to add pending attachments from the posted 
                    //      form back to viewdata so view can re-render them.
                }
                catch (RuleException rx)
                {
                    rx.CopyToModelState(ModelState, string.Empty);
                }
                catch
                {
                    ModelState.AddModelError("createFailed", "An error occurred saving the Ticket to the database. Please try again.");

                }
                if (newTicketId.HasValue)
                {
                    return RedirectToAction(MVC.TicketEditor.Display(newTicketId.Value, null));
                }
            }
            return View(new TicketCreateViewModel(Security, Settings, ticket));
        }

    }
}

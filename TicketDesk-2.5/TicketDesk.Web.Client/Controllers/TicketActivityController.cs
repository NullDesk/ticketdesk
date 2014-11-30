using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [Authorize]
    public class TicketActivityController : Controller
    {


        private TicketDeskContext Context { get; set; }
        public TicketActivityController(TicketDeskContext context)
        {
            Context = context;
        }

        public ActionResult LoadActivity(TicketActivity activity, int ticketId)
        {
            var ticket = Context.Tickets.FirstOrDefault(t => t.TicketId == ticketId);
            Context.SecurityProvider.IsTicketActivityValid(ticket, activity);
            return PartialView(string.Format("_{0}", activity), new TicketComment());
        }

        [HttpPost]
        public ActionResult AddComment(TicketComment comment)
        {
            return new EmptyResult();
        }

        public ActionResult ActivityButtons(int ticketId)
        {
            var ticket = Context.Tickets.FirstOrDefault(t => t.TicketId == ticketId);
            var activities = Context.SecurityProvider.GetValidTicketActivities(ticket);
            return PartialView("_ActivityButtons", activities);
        }
    }
}
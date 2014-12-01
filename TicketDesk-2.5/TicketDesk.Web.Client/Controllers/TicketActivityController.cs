using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Domain.Model.Extensions;

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

        public async Task<ActionResult> LoadActivity(TicketActivity activity, int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            Context.SecurityProvider.IsTicketActivityValid(ticket, activity);
            ViewBag.CommentRequired = activity.CommentRequired();
            return PartialView(string.Format("_{0}", activity), ticket);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(int ticketId, string comment)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            if (ModelState.IsValid)
            {
                Context.SecurityProvider.IsTicketActivityValid(ticket, TicketActivity.AddComment);

                var activityComment = ticket.TicketEvents.AddActivityEvent(Context.SecurityProvider.CurrentUserId,
                    TicketActivity.AddComment, comment);

                ticket.TicketEvents.Add(activityComment);

                var result = await Context.SaveChangesAsync();//save changes catches lastupdatedby and date automatically
                if (result > 0)
                {
                    return new EmptyResult();
                }
            }
            return PartialView(string.Format("_{0}", TicketActivity.AddComment), ticket);
        }

        public async Task<ActionResult> ActivityButtons(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            var activities = Context.SecurityProvider.GetValidTicketActivities(ticket);
            return PartialView("_ActivityButtons", activities);
        }
    }
}
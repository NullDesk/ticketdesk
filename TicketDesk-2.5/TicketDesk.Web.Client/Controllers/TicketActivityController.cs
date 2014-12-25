using System;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;

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

        public async Task<ActionResult> LoadActivity(TicketActivity activity, int ticketId, Guid? tempId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            Context.TicketActions.IsTicketActivityValid(ticket, activity);
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            ViewBag.TempId = tempId ?? Guid.NewGuid();
            return PartialView("_ActivityForm", ticket);
        }

        public async Task<ActionResult> ActivityButtons(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            var activities = Context.TicketActions.GetValidTicketActivities(ticket);
            return PartialView("_ActivityButtons", activities);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.AddComment(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.AddComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Assign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Assign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelMoreInfo(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.CancelMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.CancelMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.Close(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Close);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTicketInfo(
            int ticketId,
            string comment,
            string title,
            string details,
            string priority,
            string ticketType,
            string category,
            string owner,
            string tagList)
        {
            var activityFn = Context.TicketActions.EditTicketInfo(comment, title, details, priority, ticketType, category, owner, tagList, Context.Settings);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.EditTicketInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForceClose(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.ForceClose(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ForceClose);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GiveUp(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.GiveUp(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.GiveUp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyAttachments(int ticketId, string comment, Guid tempId, string deleteFiles)
        {
            //most of this action is performed directly against the storage provider, outside the business domain's control. 
            //  All the business domain has to do is record the activity log and comments
            Action<Ticket> activityFn =  ticket =>
            {
                
                //TODO: it might make sense to move the string building part of this over to the TicketDeskFileStore too?
                var sb = new StringBuilder(comment);
                if (!string.IsNullOrEmpty(deleteFiles))
                {
                    sb.AppendLine();
                    sb.AppendLine("<pre>");
                    sb.AppendLine("Removed Files:");
                    var files = deleteFiles.Split(',');
                    foreach (var file in files)
                    {
                        TicketDeskFileStore.DeleteAttachment(file, ticketId.ToString(CultureInfo.InvariantCulture), false);
                        sb.AppendLine(string.Format("    {0}", file));
                    }
                    sb.AppendLine("</pre>");
                }
                var filesAdded = ticket.CommitPendingAttachments(tempId).ToArray();
                if (filesAdded.Any())
                {
                    sb.AppendLine();
                    sb.AppendLine("<pre>");
                    sb.AppendLine("New files:");
                    foreach (var file in filesAdded)
                    {
                        sb.AppendLine(string.Format("    {0}", file));
                    }
                    sb.AppendLine("</pre>");
                }
                comment = sb.ToString();

                //perform the simple business domain functions
                var domainActivityFn = Context.TicketActions.ModifyAttachments(comment);
                domainActivityFn(ticket);
            };

            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ModifyAttachments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Pass(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Pass(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Pass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReAssign(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.ReAssign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReAssign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestMoreInfo(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.RequestMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.RequestMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReOpen(int ticketId, string comment, bool assignToMe = false)
        {
            var activityFn = Context.TicketActions.ReOpen(comment, assignToMe);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Resolve(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.Resolve(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplyMoreInfo(int ticketId, string comment, bool reactivate = false)
        {
            var activityFn = Context.TicketActions.SupplyMoreInfo(comment, reactivate);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.SupplyMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TakeOver(int ticketId, string comment, string priority)
        {
            var activityFn = Context.TicketActions.TakeOver(comment, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.TakeOver);
        }


        private async Task<ActionResult> PerformTicketAction(int ticketId, Action<Ticket> activityFn, TicketActivity activity)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            if (ModelState.IsValid)
            {
                try
                {
                    ticket.PerformAction(activityFn);
                }
                catch (SecurityException ex)
                {
                     ModelState.AddModelError("Security", ex);
                }
                var result = await Context.SaveChangesAsync(); //save changes catches lastupdatedby and date automatically
                if (result > 0)
                {
                    return new EmptyResult();//standard success case
                }
            }
            //fail case, return the view and let the client/view sort out the errors
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            return PartialView("_ActivityForm", ticket);
        }
    }
}
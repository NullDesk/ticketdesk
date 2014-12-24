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
            Context.SecurityProvider.IsTicketActivityValid(ticket, activity);
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            ViewBag.TempId = tempId ?? Guid.NewGuid();
            return PartialView("_ActivityForm", ticket);
        }

        public async Task<ActionResult> ActivityButtons(int ticketId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            var activities = Context.SecurityProvider.GetValidTicketActivities(ticket);
            return PartialView("_ActivityButtons", activities);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(int ticketId, string comment)
        {
            var activityFn = TicketAction.AddComment(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.AddComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = TicketAction.Assign(comment, assignedTo, UserDisplayInfo.GetUserInfo(assignedTo).DisplayName , priority);
            return await PerformActivity(ticketId, activityFn, TicketActivity.Assign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelMoreInfo(int ticketId, string comment)
        {
            var activityFn = TicketAction.CancelMoreInfo(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.CancelMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close(int ticketId, string comment)
        {
            var activityFn = TicketAction.Close(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.Close);
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
            Func<string, string> userNameFromId= uId => UserDisplayInfo.GetUserInfo(uId).DisplayName;
            var activityFn = TicketAction.EditTicketInfo(comment, title, details, priority, ticketType, category, owner, userNameFromId, tagList, Context.Settings);
            return await PerformActivity(ticketId, activityFn, TicketActivity.EditTicketInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForceClose(int ticketId, string comment)
        {
            var activityFn = TicketAction.ForceClose(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.ForceClose);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GiveUp(int ticketId, string comment)
        {
            var activityFn = TicketAction.GiveUp(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.GiveUp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyAttachments(int ticketId, string comment, Guid tempId, string deleteFiles)
        {
            //most of this action is performed directly against the storage provider, outside the business domain's control. 
            //  All the business domain has to do is record the activity log and comments
            Action<TicketDeskContextSecurityProviderBase,Ticket> activityFn = (security, ticket) =>
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
                var domainActivityFn = TicketAction.ModifyAttachments(comment);
                domainActivityFn(security, ticket);
            };

            return await PerformActivity(ticketId, activityFn, TicketActivity.ModifyAttachments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Pass(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = TicketAction.Pass(comment, assignedTo, UserDisplayInfo.GetUserInfo(assignedTo).DisplayName, priority);
            return await PerformActivity(ticketId, activityFn, TicketActivity.Pass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReAssign(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = TicketAction.ReAssign(comment, assignedTo, UserDisplayInfo.GetUserInfo(assignedTo).DisplayName, priority);
            return await PerformActivity(ticketId, activityFn, TicketActivity.ReAssign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestMoreInfo(int ticketId, string comment)
        {
            var activityFn = TicketAction.RequestMoreInfo(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.RequestMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReOpen(int ticketId, string comment, bool assignToMe = false)
        {
            var activityFn = TicketAction.ReOpen(comment, assignToMe);
            return await PerformActivity(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Resolve(int ticketId, string comment)
        {
            var activityFn = TicketAction.Resolve(comment);
            return await PerformActivity(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SupplyMoreInfo(int ticketId, string comment, bool reactivate = false)
        {
            var activityFn = TicketAction.SupplyMoreInfo(comment, reactivate);
            return await PerformActivity(ticketId, activityFn, TicketActivity.SupplyMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TakeOver(int ticketId, string comment, string priority)
        {
            var activityFn = TicketAction.TakeOver(comment, priority);
            return await PerformActivity(ticketId, activityFn, TicketActivity.TakeOver);
        }


        private async Task<ActionResult> PerformActivity(int ticketId, Action<TicketDeskContextSecurityProviderBase, Ticket> activityFn, TicketActivity activity)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            if (ModelState.IsValid)
            {
                try
                {
                    ticket.PerformActivity(Context.SecurityProvider, activityFn);
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
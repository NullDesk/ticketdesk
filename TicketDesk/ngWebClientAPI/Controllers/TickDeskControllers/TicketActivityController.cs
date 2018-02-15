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
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization.Controllers;


namespace ngWebClientAPI.Controllers
{
    [RoutePrefix("ticket-activity")]
    [Route("{action}")]
    //[TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    [ValidateInput(false)]
    public class TicketActivityController : Controller
    {
        //private TicketActionManager actionManager = TicketActionManager.GetInstance(new TicketDeskContextSecurityProvider());

        private TdDomainContext Context { get; set; }
        public TicketActivityController(TdDomainContext context)
        {
            Context = context;
        }

        [Route("load-activity")]
        public async Task<ActionResult> LoadActivity(TicketActivity activity, int ticketId, Guid? tempId)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            Context.TicketActions.IsTicketActivityValid(ticket, activity);
            ViewBag.CommentRequired = activity.IsCommentRequired();
            ViewBag.Activity = activity;
            ViewBag.TempId = tempId ?? Guid.NewGuid();
           // ViewBag.IsEditorDefaultHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote";
            if (activity == TicketActivity.EditTicketInfo)
            {
                await SetProjectInfoForModelAsync(ticket);
            }
            return PartialView("_ActivityForm", ticket);
        }

        private async Task SetProjectInfoForModelAsync(Ticket ticket)
        {
            var projectCount = await Context.Projects.CountAsync();
            var isMulti = (projectCount > 1);
            ViewBag.IsMultiProject = isMulti;
        }

        
        public TicketActivity ActivityButtons(int ticketId)
        {
            //WARNING! This is also used as a child action and cannot be made async in MVC 5
            var ticket = Context.Tickets.Find(ticketId);
            TicketActivity activities = Context.TicketActions.GetValidTicketActivities(ticket);
            return activities;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add-comment")]
        public async Task<string> AddComment(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.AddComment(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.AddComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("assign")]
        public async Task<string> Assign(int ticketId,  string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Assign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Assign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cancel-more-info")]
        public async Task<string> CancelMoreInfo(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.CancelMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.CancelMoreInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-ticket-info")]
        public async Task<string> EditTicketInfo(
            int ticketId,
            int projectId,
            string comment,
            string title,
            string details,
            string priority,
            string ticketType,
            string category,
            string owner,
            string tagList)
        {
            var projectName = await Context.Projects.Where(p => p.ProjectId == projectId).Select(s=>s.ProjectName).FirstOrDefaultAsync();
            var activityFn = Context.TicketActions.EditTicketInfo(comment, projectId, projectName, title, details, priority, ticketType, category, owner, tagList, Context.TicketDeskSettings);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.EditTicketInfo);
        }

        public async Task<string> ForceClose(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.ForceClose(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ForceClose);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("give-up")]
        public async Task<string> GiveUp(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.GiveUp(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.GiveUp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("pass")]
        public async Task<string> Pass(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.Pass(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.Pass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reassign")]
        public async Task<string> ReAssign(int ticketId, string comment, string assignedTo, string priority)
        {
            var activityFn = Context.TicketActions.ReAssign(comment, assignedTo, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReAssign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("request-more-info")]
        public async Task<string> RequestMoreInfo(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.RequestMoreInfo(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.RequestMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("reopen")]
        public async Task<string> ReOpen(int ticketId, string comment, bool assignToMe = false)
        {
            var activityFn = Context.TicketActions.ReOpen(comment, assignToMe);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("resolve")]
        public async Task<string> Resolve(int ticketId, string comment)
        {
            var activityFn = Context.TicketActions.Resolve(comment);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.ReOpen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("supply-more-info")]
        public async Task<string> SupplyMoreInfo(int ticketId, string comment, bool reactivate = false)
        {
            var activityFn = Context.TicketActions.SupplyMoreInfo(comment, reactivate);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.SupplyMoreInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("take-over")]
        public async Task<string> TakeOver(
            int ticketId,
            string comment,
            string priority)
        {
            var activityFn = Context.TicketActions.TakeOver(comment, priority);
            return await PerformTicketAction(ticketId, activityFn, TicketActivity.TakeOver);
        }


        private async Task<string> PerformTicketAction(int ticketId, Action<Ticket> activityFn, TicketActivity activity)
        {
            var ticket = await Context.Tickets.FindAsync(ticketId);
            ticket.PerformAction(activityFn);
            var result = await Context.SaveChangesAsync();


            //TryValidateModel(ticket);
            Console.WriteLine("Here we are");
            /*  
             Had to comment this out because for some reason 
             the validation fails. 
             */

            /*
            if (true)
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
            */
            //fail case, return the view and let the client/view sort out the errors
            //ViewBag.CommentRequired = activity.IsCommentRequired();
            //ViewBag.Activity = activity;
            // ViewBag.IsEditorDefaultHtml = Context.TicketDeskSettings.ClientSettings.GetDefaultTextEditorType() == "summernote";
            return "Returning";
        }
    }
}
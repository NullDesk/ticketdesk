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
using TicketDesk.Domain.Services;
using System.Web.Mvc;
using TicketDesk.Domain.Utilities;
using TicketDesk.Domain.Models;
using TicketDesk.Web.Client.Helpers;
using System.ComponentModel.Composition;

namespace TicketDesk.Web.Client.Controllers
{
    [HandleError]
    [NoCache]
    [ValidateInput(false)]
    [Export("TicketEditor", typeof(IController))]
    public partial class TicketEditorController : ApplicationController
    {
        public ITicketService Tickets { get; private set; }
        public SettingsService Settings { get; private set; }

        [ImportingConstructor]
        public TicketEditorController(ITicketService ticketService, ISecurityService security, SettingsService settings)
            : base(security)
        {
            Tickets = ticketService;
            Settings = settings;
        }

        [ValidateInput(false)]
        public virtual ContentResult MarkdownPreview(string data)
        {
            var c = new ContentResult();

            var md = new Markdown();
            c.Content = "<style>body{font-size: 8pt;font-family: Verdana, Helvetica, Sans-Serif;margin: 0;padding: 2px;color: #555;}\n</style>";
            c.Content += md.Transform(data, true);

            return c;
        }


        [Authorize]
        public virtual ActionResult Display(int id, string activity)
        {
            var model = Tickets.GetTicket(id);
            ViewData.Add("Security", Security);
            if (string.IsNullOrEmpty(activity))
            {
                activity = "ActivityButtons";
            }

            TicketActivity activityEn;

            if (activity == "invalid" || activity == "ActivityButtons")
            {
                activityEn = TicketActivity.NoChange;
            }
            else if (activity == "Assign")//assign has different security requirements depending on who ticket is currently assigned to
            {
                if (string.IsNullOrEmpty(model.AssignedTo))
                {
                    activityEn = TicketActivity.Assign;
                }
                else if (model.AssignedTo == Security.CurrentUserName)
                {
                    activityEn = TicketActivity.Pass;
                }
                else
                {
                    activityEn = TicketActivity.ReAssign;
                }
            }
            else
            {
                activityEn = (TicketActivity)Enum.Parse(typeof(TicketActivity), activity);
            }

            //TODO: See about a filter for the security check
            if (!Tickets.CheckSecurityForTicketActivity(model, activityEn))
            {
                // TODO: if this failed, it is probably because something changed to make the requested activity no longer available (ticket state changed),
                //         need to show the error panel                        
                if (IsItReallyRedirectFromAjax())
                {
                    TempData["IsRedirectFromAjax"] =IsItReallyRedirectFromAjax();
                    return RedirectToAction(MVC.TicketEditor.Display(id, string.Empty));
                }
            }

            SetupActivityViewData(activity, model);

            if (IsItReallyRedirectFromAjax())
            {
                return PartialView(string.Format("~/Views/TicketEditor/Controls/{0}.ascx", activity), model);
            }

            return View(model);

        }

        private void SetupActivityViewData(string activity, Ticket ticket)
        {

            ViewData.Add("activity", activity);
            //TODO: Need a more elaborate way to map activity name to display name text
            var activityDisplayName = (activity == "ActivityButtons") ? "Choose Activity" : activity.ConvertPascalCaseToFriendlyString();
            ViewData.Add("activityDisplayName", activityDisplayName);


        }

        [Authorize]
        public virtual ActionResult RefreshHistory(int id)
        {
            var model = Tickets.GetTicket(id);
            if (IsItReallyRedirectFromAjax())
            {

                return PartialView(MVC.TicketEditor.Views.Controls.ActivityHistory, model);
            }
            else
            {
                return RedirectToAction(MVC.TicketEditor.Display(id, string.Empty));

            }
        }

        [Authorize]
        public virtual ActionResult RefreshStats(int id)
        {
            var model = Tickets.GetTicket(id);
            if (IsItReallyRedirectFromAjax())
            {

                return PartialView(MVC.TicketEditor.Views.Controls.TicketStats, model);
            }
            else
            {
                return RedirectToAction(MVC.TicketEditor.Display(id, string.Empty));
            }
        }

        [Authorize]
        public virtual ActionResult RefreshDetails(int id)
        {
            var model = Tickets.GetTicket(id);
            if (IsItReallyRedirectFromAjax())
            {

                return PartialView(MVC.TicketEditor.Views.Controls.Details, model);
            }
            else
            {
                return RedirectToAction(MVC.TicketEditor.Display(id, string.Empty));
            }
        }


        [Authorize]
        public virtual ActionResult RefreshAttachments(int id)
        {
            var model = Tickets.GetTicket(id);
            if (IsItReallyRedirectFromAjax())
            {

                return PartialView(MVC.TicketEditor.Views.Controls.Attachments, model);
            }
            else
            {
                return RedirectToAction(MVC.TicketEditor.Display(id, string.Empty));
            }
        }

        [Authorize]
        public virtual ActionResult AddComment(int id, string comment, bool? resolve)
        {
            var model = Tickets.GetTicket(id);

            if (resolve.HasValue && resolve.Value)
            {
                return PerformActivity(id, "Resolve", comment);
            }

            return PerformActivity(id, "AddComment", comment);
        }

        [Authorize]
        public virtual ActionResult Resolve(int id, string comment)
        {
            return PerformActivity(id, "Resolve", comment);
        }

        [Authorize]
        public virtual ActionResult TakeOver(int id, string comment, string priority)
        {
            //TODO: We need a setting to indicate if priority is required at takeover point
            return PerformActivity(id, "TakeOver", comment, priority);
        }

        [Authorize]
        public virtual ActionResult Assign(int id, string comment, string assignedTo, string priority)
        {
            //TODO: We need a setting to indicate if priority is required at assigning
            return PerformActivity(id, "Assign", comment, assignedTo, priority);
        }

        [Authorize]
        public virtual ActionResult Close(int id, string comment)
        {
            return PerformActivity(id, "Close", comment);
        }

        [Authorize]
        public virtual ActionResult ForceClose(int id, string comment)
        {
            return PerformActivity(id, "ForceClose", comment);
        }

        [Authorize]
        public virtual ActionResult GiveUp(int id, string comment)
        {
            return PerformActivity(id, "GiveUp", comment);
        }

        [Authorize]
        public virtual ActionResult ReOpen(int id, string comment, bool? assignToMe, bool? ownedByMe)
        {
            return PerformActivity(id, "ReOpen", comment, assignToMe, ownedByMe);
        }

        [Authorize]
        public virtual ActionResult RequestMoreInfo(int id, string comment)
        {
            return PerformActivity(id, "RequestMoreInfo", comment);
        }

        [Authorize]
        public virtual ActionResult SupplyMoreInfo(int id, string comment, bool? markActive)
        {
            if (!markActive.HasValue)
            {
                markActive = false;
            }
            return PerformActivity(id, "SupplyMoreInfo", comment, markActive.Value);
        }

        [Authorize]
        public virtual ActionResult CancelMoreInfo(int id, string comment)
        {
            return PerformActivity(id, "CancelMoreInfo", comment);
        }


        [Authorize]
        public virtual ActionResult ModifyAttachments
        (
            int id,
            string comment,
            [ModelBinder(typeof(AttachmentsModelBinder))] 
            List<TicketAttachment> attachments)
        {

            return PerformActivity(id, "ModifyAttachments", comment, attachments);
        }

        [Authorize]
        [ValidateOnlyIncomingValues]
        public virtual ActionResult EditTicketInfo(int id, string comment)
        {

            var ticket = Tickets.GetTicket(id);
            UpdateModel<Ticket>(ticket, new[] { "Title", "Details", "TagList", "Priority", "Category", "Type", "Owner" });

            return PerformActivity(ticket, "EditTicketInfo", comment);
        }






        private ActionResult PerformActivity(int ticketId, string activityName, string commentContent, params object[] args)
        {
            return PerformActivity(Tickets.GetTicket(ticketId), activityName, commentContent, args);
        }

        private ActionResult PerformActivity(Ticket model, string activityName, string commentContent, params object[] args)
        {
            if (ModelState.IsValid)//if the modelstate is already invalid, don't perform the activity, just move on to re-rendering the view
            {
                try
                {
                    bool result = DoTicketActivity(activityName, model, commentContent, args);
                    if (!result)
                    {
                        throw new ApplicationException("Ticket Activity Failed");
                    }
                }
                catch (RuleException rx)
                {
                    if (rx.Errors.AllKeys.Contains("noAuth"))
                    {
                        //TODO: Authorization to perform the action failed, need to show the
                        //          error activity panel and cancel copy to model state

                    }
                    rx.CopyToModelState(ModelState, string.Empty);
                }
                catch
                {
                    ModelState.AddModelError("unknownSaveError", "Unknown error, please try again");
                }
            }
            if (ModelState.IsValid)
            {
                TempData["IsRedirectFromAjax"] = IsItReallyRedirectFromAjax();
                return RedirectToAction(MVC.TicketEditor.Display(model.TicketId, string.Empty));
            }
            else
            {
                if (IsItReallyRedirectFromAjax())
                {
                    SetupActivityViewData(activityName, model);
                    return PartialView(string.Format("~/Views/TicketEditor/Controls/{0}.ascx", activityName), model);
                }
                else
                {
                    return RedirectToAction(MVC.TicketEditor.Display(model.TicketId, string.Empty));
                }
            }
        }

        private bool DoTicketActivity(string activityName, Ticket ticket, string comments, params object[] args)
        {
            //TODO: The methods called on the ticket service are supplying the user name, but the ticket service has a reference to security service and can infer that
            bool result = false;
            switch (activityName)
            {
                case "AddComment":
                    result = Tickets.AddCommentToTicket(ticket, comments);
                    break;
                case "SupplyMoreInfo":
                    bool markActive = (bool)args[0];
                    result = Tickets.SupplyMoreInfoForTicket(ticket, comments, markActive);
                    break;
                case "RequestMoreInfo":
                    result = Tickets.RequestMoreInfoForTicket(ticket, comments);
                    break;
                case "CancelMoreInfo":
                    result = Tickets.CancelMoreInfoForTicket(ticket, comments);
                    break;
                case "TakeOver":
                    string tpriority = (args != null && args.Length == 1) ? args[0] as string : null;
                    result = Tickets.TakeOverTicket(ticket, comments, tpriority);
                    break;
                case "Assign":
                    string assignTo = (args != null && args.Length > 0) ? args[0] as string : null;
                    string apriority = (args != null && args.Length > 1) ? args[1] as string : null;
                    result = Tickets.AssignTicket(ticket, comments, assignTo, apriority);
                    break;
                case "Resolve":
                    result = Tickets.ResolveTicket(ticket, comments);
                    break;
                case "Close":
                    result = Tickets.CloseTicket(ticket, comments, false);
                    break;
                case "ForceClose":
                    result = Tickets.CloseTicket(ticket, comments, true);
                    break;
                case "GiveUp":
                    result = Tickets.GiveUpTicket(ticket, comments);
                    break;
                case "ReOpen":
                    bool? assignToMe = (args != null && args.Length > 0) ? args[0] as bool? : false;
                    bool? ownedByMe = (args != null && args.Length > 1) ? args[1] as bool? : false;
                    if (!assignToMe.HasValue)
                    {
                        assignToMe = false;
                    }
                    if (!ownedByMe.HasValue)
                    {
                        ownedByMe = false;
                    }

                    if (!Security.IsTdStaff())
                    {
                        if (ticket.Owner != Security.CurrentUserName)
                        {
                            ownedByMe = true;
                        }
                    }

                    result = Tickets.ReOpenTicket(ticket, comments, assignToMe.Value, ownedByMe.Value);
                    break;
                case "ModifyAttachments":

                    var attachments = args[0] as List<TicketAttachment>;
                    result = Tickets.ModifyAttachmentsForTicket(ticket, comments, attachments);
                    //TODO: on successful result we need to remove the pending files. The model has no notion of "pending files" though so this needs to be done seperately
                    break;
                case "EditTicketInfo":
                    result = Tickets.EditTicketDetails(ticket, comments);
                    break;
            }
            return result;

        }
    }
}
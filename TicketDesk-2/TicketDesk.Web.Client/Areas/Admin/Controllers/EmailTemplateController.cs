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
using TicketDesk.Web.Client.Models;
using System.IO;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using TicketDesk.Domain.Models;
using System.Configuration;
using System.Net.Mail;
using TicketDesk.Web.Client.Controllers;
using TicketDesk.Web.Client.Helpers;
namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    [HandleError]
    [Export("EmailTemplate", typeof(IController))]
    public partial class EmailTemplateController : ApplicationController
    {

        [ImportingConstructor]
        public EmailTemplateController(): base(MefHttpApplication.ApplicationContainer.GetExportedValue<ISecurityService>()){}
        

        #region Admin preview stuff

        [Import(typeof(INotificationSendingService))]
        private TicketDesk.Domain.Services.NotificationSendingService noteService { get; set; }


        [AuthorizeAdminOnly]
        public virtual ActionResult Index()
        {
            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Index();
            }

            return View();
        }
        [AuthorizeAdminOnly]
        [HttpPost]
        public virtual ActionResult Index(int? id, string mode)
        {
            if (id == null)
            {
                ModelState.AddModelError("id", "Ticket ID invalid");
                return View();
            }
            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Index();
            }

            ActionResult a = null;

            switch (mode)
            {
                case "html":
                    a = MVC.Admin.EmailTemplate.Actions.DisplayHtml(id.Value);
                    break;
                case "outlook":
                    a = MVC.Admin.EmailTemplate.Actions.DisplayOutlookHtml(id.Value);
                    break;
                case "text":
                    a = MVC.Admin.EmailTemplate.Actions.DisplayText(id.Value);
                    break;
                default:
                    a = MVC.Admin.AdminHome.Index();
                    break;
            }
            return RedirectToAction(a);
        }

        [AuthorizeAdminOnly]
        public virtual ContentResult ProcessWaitingNotesNow()
        {
            var c = new ContentResult();
            c.ContentType = "text/plain";
            if (!Security.IsTdAdmin())
            {
                c.Content = "Not Allowed - Contact your administrator";
            }
            else
            {
                noteService.ProcessWaitingTicketEventNotifications();
                c.Content = "All Pending emails have been sent";
            }

            return c;
        }

        [AuthorizeAdminOnly]
        public virtual ActionResult DisplayHtml(int id)
        {
            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Index();
            }

            return GetEmailPreview(id, MVC.Admin.EmailTemplate.Views.TicketNotificationHtmlEmailTemplate);
        }

        [AuthorizeAdminOnly]
        public virtual ActionResult DisplayOutlookHtml(int id)
        {
            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Index();
            }

            return GetEmailPreview(id, MVC.Admin.EmailTemplate.Views.TicketNotificationOutlookHtmlEmailTemplate);
        }

        [AuthorizeAdminOnly]
        public virtual ActionResult DisplayText(int id)
        {
            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Index();
            }

            Response.ContentType = "text/plain";
            return GetEmailPreview(id, MVC.Admin.EmailTemplate.Views.TicketNotificationTextEmailTemplate);

        }

        private ActionResult GetEmailPreview(int id, string view)
        {
            var ticketService = new TicketService(Security, new TicketDesk.Domain.Repositories.TicketRepository(), null, null);
            var ticket = ticketService.GetTicket(id);
            var comment = ticket.TicketComments.SingleOrDefault(tc => tc.CommentId == ticket.TicketComments.Max(c => c.CommentId));
            if (comment != null)
            {
                var note = comment.TicketEventNotifications.FirstOrDefault();
                ViewData.Add("siteRootUrl", AppSettings.SiteRootUrlForEmail);
                ViewData.Add("firstUnsentCommentId", note.CommentId);
                ViewData.Add("formatForEmail", true);
                return View(view, ticket);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion

        #region Email Automation Actions

        public string GenerateTicketNotificationTextEmailBody(TicketEventNotification notification, int firstUnsentCommentId)
        {
            var templateName = MVC.Admin.EmailTemplate.Views.TicketNotificationTextEmailTemplate;
            return GenerateTicketNotificationEmailBody(notification, firstUnsentCommentId, templateName);
        }

        public string GenerateTicketNotificationHtmlEmailBody(TicketEventNotification notification, int firstUnsentCommentId)
        {
            var forOutlook = (AppSettings.EnableOutlookFriendlyHtmlEmail);
            var templateToRender = (forOutlook) ? MVC.Admin.EmailTemplate.Views.TicketNotificationOutlookHtmlEmailTemplate : MVC.Admin.EmailTemplate.Views.TicketNotificationHtmlEmailTemplate;

            return GenerateTicketNotificationEmailBody(notification, firstUnsentCommentId, templateToRender);
        }

        private string GenerateTicketNotificationEmailBody(TicketEventNotification notification, int firstUnsentCommentId, string templateToRender)
        {
            this.Security.GetCurrentUserName = delegate() { return notification.NotifyUser; };
            var ticket = notification.TicketComment.Ticket;

            var vd = new ViewDataDictionary(ticket);
            vd.Add("siteRootUrl", AppSettings.SiteRootUrlForEmail);
            vd.Add("firstUnsentCommentId", firstUnsentCommentId);
            vd.Add("formatForEmail", true);
            using (StringWriter sw = new StringWriter())
            {
                var fakeResponse = new HttpResponse(sw);
                var fakeContext = new HttpContext(new HttpRequest("", AppSettings.SiteRootUrlForEmail, ""), fakeResponse);
                var fakeControllerContext = new ControllerContext
                (
                    new HttpContextWrapper(fakeContext),
                    new RouteData(),
                    this
                );
                fakeControllerContext.RouteData.Values.Add("controller", "EmailTemplate");

                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(fakeControllerContext, templateToRender);
                ViewContext vc = new ViewContext(fakeControllerContext, new FakeView(), vd, new TempDataDictionary(), sw);

                HtmlHelper h = new HtmlHelper(vc, new ViewPage());

                h.RenderPartial(templateToRender, ticket, vd);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion

        public class FakeView : IView
        {
            #region IView Members
            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}

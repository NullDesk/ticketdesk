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

namespace TicketDesk.Web.Client.Controllers
{
    [HandleError]
    [Export("EmailTemplate", typeof(IController))]
    public partial class EmailTemplateController : Controller
    {

        #region temporary stuff needed only during development (or needs to go to unit testing)
        
        [Import(typeof(INotificationSendingService))]
        private TicketDesk.Domain.Services.NotificationSendingService noteService { get; set; }

        public string Test()
        {
            noteService.ProcessWaitingTicketEventNotifications();
            return "Notes have been processed";
        }

        #endregion


        public string GenerateTicketNotificationTextEmailBody(TicketEventNotification notification, string urlForTicket, int firstUnsentCommentId)
        {
            var templateName = "~/Views/EmailTemplate/TicketNotificationTextEmailTemplate.ascx";
            return GenerateTicketNotificationEmailBody(notification, urlForTicket, firstUnsentCommentId, templateName);
        }

        public string GenerateTicketNotificationHtmlEmailBody(TicketEventNotification notification, string urlForTicket, int firstUnsentCommentId)
        {
            var templateToRender = "~/Views/EmailTemplate/TicketNotificationHtmlEmailTemplate.ascx";
            return GenerateTicketNotificationEmailBody(notification, urlForTicket, firstUnsentCommentId, templateToRender);
        }


        private string GenerateTicketNotificationEmailBody(TicketEventNotification notification, string urlForTicket, int firstUnsentCommentId, string templateToRender)
        {
            var ticket = notification.TicketComment.Ticket;
           
            var vd = new ViewDataDictionary(notification);
            vd.Add("urlForTicket", urlForTicket);
            vd.Add("firstUnsentCommentId", firstUnsentCommentId);

            using (StringWriter sw = new StringWriter())
            {
                var fakeResponse = new HttpResponse(sw);
                var fakeContext = new HttpContext(new HttpRequest("", "http://mySomething/", ""), fakeResponse);
                var fakeControllerContext = new ControllerContext
                (
                    new HttpContextWrapper(fakeContext),
                    new RouteData(),
                    this
                );
                fakeControllerContext.RouteData.Values.Add("controller", "EmailTemplate");
                
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(fakeControllerContext, templateToRender);
                ViewContext vc = new ViewContext(fakeControllerContext, new FakeView(), new ViewDataDictionary(), new TempDataDictionary(), sw);

                HtmlHelper h = new HtmlHelper(vc, new ViewPage());

                h.RenderPartial(templateToRender, notification);
                
                return sw.GetStringBuilder().ToString();
            }
        }

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

// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications.Common;
using TicketDesk.PushNotifications.Common.Model;
using TicketDesk.Search.Azure;
using TicketDesk.Search.Common;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/push-notification-settings")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class PushNotificationSettingsController : Controller
    {
        private TdPushNotificationContext Context { get; set; }
        public PushNotificationSettingsController(TdPushNotificationContext context)
        {
            Context = context;
        }

        public ActionResult Index()
        {
            var dbSetting = Context.PushNotificationSettings;
            
            return View(dbSetting);
        }

        [HttpPost]
        public ActionResult Index(ApplicationPushNotificationSetting settings)
        {
            var dbSetting = Context.PushNotificationSettings;
            if (ModelState.IsValid && TryUpdateModel(dbSetting))
            {
            
                Context.SaveChanges();
            }
            return View(dbSetting);
        }
    }
}
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

using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Migrations;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Migrations;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Migrations;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/data-management")]
    [Route("{action=index}")]
    [TdAuthorize(Roles = "TdAdministrators")]
    public class DataManagementController : Controller
    {
        private TdIdentityContext IdentityContext { get; set; }
        private TdPushNotificationContext PushNotificationContext { get; set; }
        public DataManagementController(TdIdentityContext identityContext, TdPushNotificationContext pushNotificationContext)
        {
            IdentityContext = identityContext;
            PushNotificationContext = pushNotificationContext;
        }

        [Route("demo")]
        public ActionResult Demo()
        {
            return View();
        }

        [Route("remove-demo-data")]
        public ActionResult RemoveDemoData()
        {
            using (var ctx = new TdDomainContext(null))
            {
                DemoDataManager.RemoveAllData(ctx);
            }
            DemoIdentityDataManager.RemoveAllIdentity(IdentityContext);
            DemoPushNotificationDataManager.RemoveAllPushNotificationData(PushNotificationContext);
            ViewBag.DemoDataRemoved = true;
            return View("Demo");
        }

        [Route("create-demo-data")]
        public ActionResult CreateDemoData()
        {
            using (var ctx = new TdDomainContext(null))
            {
                DemoDataManager.SetupDemoData(ctx);
            }
            DemoIdentityDataManager.SetupDemoIdentityData(IdentityContext);
            DemoPushNotificationDataManager.SetupDemoPushNotificationData(PushNotificationContext);
            ViewBag.DemoDataCreated = true;
            return View("Demo");
        }
    }
}
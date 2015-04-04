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

using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Migrations;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Infrastructure;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/data-management")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class DataManagementController : Controller
    {
        private TdIdentityContext IdentityContext { get; set; }
        public DataManagementController(TdIdentityContext identityContext)
        {
            IdentityContext = identityContext;
        }

        [Route("demo")]
        public ActionResult Demo()
        {
            return View();
        }

        [Route("remove-demo-data")]
        public ActionResult RemoveDemoData()
        {
            using (var ctx = new TdContext(null))
            {
                DemoDataManager.RemoveAllData(ctx);
            }
            DemoIdentityDataManager.RemoveAllIdentity(IdentityContext);
            ViewBag.DemoDataRemoved = true;
            return View("Demo");
        }

        [Route("create-demo-data")]
        public ActionResult CreateDemoData()
        {
            using (var ctx = new TdContext(null))
            {
                DemoDataManager.SetupDemoData(ctx);
            }
            DemoIdentityDataManager.SetupDemoIdentityData(IdentityContext);

            ViewBag.DemoDataCreated = true;
            return View("Demo");
        }
    }
}
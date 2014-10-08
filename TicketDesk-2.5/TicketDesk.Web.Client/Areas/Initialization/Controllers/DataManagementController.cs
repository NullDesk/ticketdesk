using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client.Areas.Initialization.Controllers
{
    public class DataManagementController : Controller
    {
        public ActionResult Upgrade()
        {
            return View();
        }

        public ActionResult UpgradeNow()
        {
            TicketDeskLegacyDatabaseInitializer<TicketDeskContext>.InitDatabase();

            using (var ctx = new TicketDeskContext())
            {
                Database.SetInitializer<TicketDeskContext>(new MigrateDatabaseToLatestVersion<TicketDeskContext, TicketDesk.Domain.Migrations.Configuration>());
                ctx.Database.Initialize(true);
            }
            ViewBag.DbUpgraded = true;
            var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter.Instance);
            }
            return View("Upgrade");
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateNow()
        {
            using (var ctx = new TicketDeskContext())
            {
                Database.SetInitializer<TicketDeskContext>(new MigrateDatabaseToLatestVersion<TicketDeskContext, TicketDesk.Domain.Migrations.Configuration>());
                ctx.Database.Initialize(true);
            }
            ViewBag.DbCreated = true;
            var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter.Instance);
            }
            return View("Create");
        }

        
        public ActionResult MigrateMembershipToIdentity()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<TicketDeskUserManager>();
            var roleManager = HttpContext.GetOwinContext().Get<TicketDeskRoleManager>();
            var ctx = HttpContext.GetOwinContext().Get<TicketDeskIdentityContext>();
            LegacySecurityMigrator.MigrateSecurity(ctx, userManager, roleManager);
            ViewBag.UsersMigrated = true;
            return View("Upgrade");
        }
    }
}
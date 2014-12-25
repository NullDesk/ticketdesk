using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;
using TicketDesk.Domain.Migrations;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Infrastructure;

namespace TicketDesk.Web.Client.Areas.Initialization.Controllers
{
    public class DataManagementController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskRoleManager RoleManager { get; set; }
        private TicketDeskIdentityContext IdentityContext { get; set; }
        public DataManagementController(TicketDeskUserManager userManager, TicketDeskRoleManager roleManager, TicketDeskIdentityContext context)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            IdentityContext = context;

        }

        public ActionResult Upgrade()
        {
            return View();
        }

        public ActionResult UpgradeNow()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                TicketDeskLegacyDatabaseInitializer<TicketDeskContext>.InitDatabase(ctx);
                Database.SetInitializer<TicketDeskContext>(new MigrateDatabaseToLatestVersion<TicketDeskContext, Domain.Migrations.Configuration>(true));
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
            using (var ctx = new TicketDeskContext(null))
            {
                Database.SetInitializer<TicketDeskContext>(
                    new MigrateDatabaseToLatestVersion<TicketDeskContext, Domain.Migrations.Configuration>(true));
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
            LegacySecurityMigrator.MigrateSecurity(IdentityContext, UserManager, RoleManager);
            ViewBag.UsersMigrated = true;
            return View("Upgrade");
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult RemoveDemoData()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                DemoDataManager.RemoveAllData(ctx);

            }
            DemoIdentityDataManager.RemoveAllIdentity(IdentityContext);
            ViewBag.DemoDataRemoved = true;
            return View("Demo");
        }

        public ActionResult CreateDemoData()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                DemoDataManager.SetupDemoData(ctx);

            }
            DemoIdentityDataManager.SetupDemoIdentityData(IdentityContext);

            ViewBag.DemoDataCreated = true;
            return View("Demo");
        }
    }
}
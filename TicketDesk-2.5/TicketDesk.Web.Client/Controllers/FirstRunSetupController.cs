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

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using Configuration = TicketDesk.Domain.Migrations.Configuration;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("first-run-setup")]
    [Route("{action=index}")]
    [WhenSetupEnabled]
    public class FirstRunSetupController : Controller
    {
        private SystemInfoViewModel Model { get; set; }
        public FirstRunSetupController()
        {
            Model = new SystemInfoViewModel();
        }

        // GET: FirstRunSetup
        [Route("", Name = "first-run-setup")]
        [Route("index")]
        public ActionResult Index()
        {
         
            return View();
        }

        [Route("legacy-migration")]
        public ActionResult LegacyMigration()
        {
            var identityCtx = DependencyResolver.Current.GetService<TicketDeskIdentityContext>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            if (LegacySecurityMigrator.MigrateSecurity(identityCtx, userManager, roleManager))
            {
                LegacySecurityMigrator.RemoveLegacyMembershipObjects(identityCtx);
            }
            return RedirectToAction("Index");
        }

        [Route("upgrade-database")]
        public ActionResult UpgradeDatabase()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                TicketDeskLegacyDatabaseInitializer<TicketDeskContext>.InitDatabase(ctx);

            }
            using (var ctx = new TicketDeskContext(null))
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<TicketDeskContext, Configuration>(true));
                ctx.Database.Initialize(true);
            }

            var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter.Instance);
            }
            
            return RedirectToAction("Index");
        }

        [Route("create-database")]
        public ActionResult CreateDatabase()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<TicketDeskContext, Configuration>(true));
                ctx.Database.Initialize(true);
            }
            var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter.Instance);
            }
            
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult AzureInfo()
        {
            if (!Model.AzureInfo.HasAzureServices)
            {
                return new EmptyResult();
            }
            return PartialView("_AzureInfo", Model);
            
        }

        [ChildActionOnly]
        public ActionResult DatabaseInfo()
        {
            return PartialView("_DatabaseInfo", Model);
        }

        [ChildActionOnly]
        public ActionResult NewDatabase()
        {
            if (Model.DatabaseStatus.IsDatabaseReady || Model.DatabaseStatus.IsLegacyDatabase)
            {
                return new EmptyResult();
            }

            if (Model.AzureInfo.IsAzureWebSite)
            {
                    ViewBag.ErrorAzureDbDoesNotExist = !Model.DatabaseStatus.DatabaseExists;
                    ViewBag.WarnNotAnAzureDb = !Model.AzureInfo.IsSqlAzure;
            }
            return View("_NewDatabase",Model);

        }

        [ChildActionOnly]
        public ActionResult LegacyDatabase()
        {
            if (Model.DatabaseStatus.IsDatabaseReady || !Model.DatabaseStatus.IsLegacyDatabase)
            {
                return new EmptyResult();
            }
             return View("_LegacyDatabase", Model);
        }

        [ChildActionOnly]
        public ActionResult LegacySecurity()
        {
            if (!Model.DatabaseStatus.IsDatabaseReady || !Model.DatabaseStatus.HasLegacySecurityObjects)
            {
                return new EmptyResult();
            }
            return View("_LegacySecurity",Model);
        }

        [ChildActionOnly]
        public ActionResult SetupCompleteInfo()
        {

            if (!Model.DatabaseStatus.IsDatabaseReady || !Model.DatabaseStatus.IsCompatibleWithEfModel || Model.DatabaseStatus.HasLegacySecurityObjects)
            {
                return new EmptyResult();
            }
            return View("_SetupCompleteInfo", Model);
        }
    }
    
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;
using TicketDesk.Domain.Search.AzureSearch;
using TicketDesk.IO;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;
using Configuration = TicketDesk.Domain.Migrations.Configuration;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("first-run-setup")]
    [Route("{action=index}")]
    [WhenSetupEnabled]
    public class FirstRunSetupController : Controller
    {
        public FirstRunSetupController()
        {
            //TODO: what about demo mode?

            IsAzureWebSite = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            ConnectionString = ConfigurationManager.ConnectionStrings["TicketDesk"].ConnectionString;

            var conn = GetMasterCatalogConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SERVERPROPERTY('Edition')";
            conn.Open();
            var edition = cmd.ExecuteScalar() as string;
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT SERVERPROPERTY('IsLocalDB')";
            var ldb = cmd.ExecuteScalar() as int?;
            conn.Close();
            if (edition != null)
            {
                DbEdition = edition;
                IsSqlAzure = edition.IndexOf("azure", StringComparison.InvariantCultureIgnoreCase) >= 0;
            }
            IsLocalDb = ldb.HasValue;

        }

        private string DbEdition { get; set; }
        private bool IsLocalDb { get; set; }
        private bool IsSqlAzure { get; set; }
        private bool IsAzureWebSite { get; set; }
        private string ConnectionString { get; set; }

        private SqlConnection GetMasterCatalogConnection()
        {

            var builder = new SqlConnectionStringBuilder(ConnectionString);
            builder.AttachDBFilename = "";
            builder.InitialCatalog = "Master";
            return new SqlConnection(builder.ToString());

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
            var hasAzureServices = false;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
            {
                ViewBag.AzureWebSiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
                ViewBag.IsAzureWebSite = IsAzureWebSite;
                hasAzureServices = true;
            }
            var azSearchInfo = AzureSearchConector.TryGetInfoFromConnectionString() ??
                                   AzureSearchConector.TryGetInfoFromAppSettings();

            if (azSearchInfo != null)
            {
                hasAzureServices = true;
                ViewBag.AzureSearchService = azSearchInfo.ServiceName;
            }
            var azStore = !string.IsNullOrEmpty(AzureConnectionHelper.ConfigManagerConnString) ? AzureConnectionHelper.ConfigManagerConnString : AzureConnectionHelper.CloudConfigConnString;

            if (!string.IsNullOrEmpty(azStore))
            {
                try
                {
                    var parts = azStore
                        .Split(';')
                        .Select(p => p.Split('='))
                        .Select(t => new { key = t[0], value = t[1] }).ToList();

                    var service = parts.FirstOrDefault(p => p.key.Equals("AccountName", StringComparison.InvariantCultureIgnoreCase));
                    if (service != null)
                    {
                        ViewBag.AzureStorageService = service.value;
                        hasAzureServices = true;
                    }
                }
                catch { }

            }
            if (IsSqlAzure)
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                ViewBag.Database = builder.InitialCatalog;
                hasAzureServices = true;
            }
            ViewBag.HasAzureServices = hasAzureServices;
            return PartialView("_AzureInfo");
        }

        [ChildActionOnly]
        public ActionResult DatabaseInfo()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            var dsource = builder.DataSource.Split('\\');
            ViewBag.ServerName = dsource[0];
            ViewBag.SqlInstance = dsource.Length > 1 ? dsource[1] : string.Empty;
            //note: localdb doesn't show as a user instance!
            ViewBag.IsFileDatabase = !string.IsNullOrEmpty(builder.AttachDBFilename);
            ViewBag.Database = ViewBag.IsFileDatabase ? builder.AttachDBFilename : builder.InitialCatalog;

            if (DbEdition != null)
            {
                ViewBag.ServerVersion = DbEdition;
                ViewBag.IsAzureSql = IsSqlAzure;
            }
            ViewBag.ServerIsLocalDb = IsLocalDb;
            ViewBag.DatabaseReady = DatabaseConfig.IsDatabaseReady;
            return PartialView("_DatabaseInfo");
        }

        [ChildActionOnly]
        public ActionResult NewDatabase()
        {
            if (DatabaseConfig.IsDatabaseReady || DatabaseConfig.IsLegacyDatabase())
            {
                return new EmptyResult();
            }

            if (IsAzureWebSite)
            {
                using (var ctx = new TicketDeskContext(null))
                {
                    ViewBag.ErrorAzureDbDoesNotExist = !ctx.Database.Exists();
                    ViewBag.WarnNotAnAzureDb = !IsSqlAzure;
                }
            }
            return View("_NewDatabase");

        }

        [ChildActionOnly]
        public ActionResult LegacyDatabase()
        {
            if (DatabaseConfig.IsDatabaseReady || !DatabaseConfig.IsLegacyDatabase())
            {
                return new EmptyResult();
            }
             return View("_LegacyDatabase");
        }

        [ChildActionOnly]
        public ActionResult LegacySecurity()
        {
            if (!DatabaseConfig.IsDatabaseReady || !DatabaseConfig.HasLegacySecurity())
            {
                return new EmptyResult();
            }
            return View("_LegacySecurity");
        }

        [ChildActionOnly]
        public ActionResult SetupCompleteInfo()
        {
            var compatible = false;
            using (var ctx = new TicketDeskContext(null))
            {
               compatible = ctx.Database.CompatibleWithModel(false);
            }
            if (!DatabaseConfig.IsDatabaseReady || !compatible || DatabaseConfig.HasLegacySecurity())
            {
                return new EmptyResult();
            }
            return View("_SetupCompleteInfo");
        }
    }
    
}
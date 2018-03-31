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

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Domain;
using TicketDesk.Domain.Legacy;
using TicketDesk.Domain.Migrations;
using TicketDesk.Domain.Model;
using TicketDesk.PushNotifications;
using TicketDesk.PushNotifications.Model;
using TicketDesk.Search.Common;
using TicketDesk.Web.Client.Models;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client.Controllers
{


    [RoutePrefix("first-run-setup")]
    [Route("{action=index}")]
    [WhenSetupEnabled]
    public class FirstRunSetupController : Controller
    {
        private TicketDeskUserManager UserManager { get; set; }
        private TicketDeskSignInManager SignInManager { get; set; }

        private SystemInfoViewModel Model { get; set; }
        public FirstRunSetupController(
            TicketDeskUserManager userManager,
            TicketDeskSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
            var identityCtx = DependencyResolver.Current.GetService<TdIdentityContext>();
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            var roleManager = DependencyResolver.Current.GetService<TicketDeskRoleManager>();
            if (LegacySecurityMigrator.MigrateSecurity(identityCtx, userManager, roleManager))
            {
                LegacySecurityMigrator.RemoveLegacyMembershipObjects(identityCtx);
            }
            return RedirectToAction("Index");
        }

        [Route("upgrade-database")]
        public async Task<ActionResult> UpgradeDatabase()
        {


            LegacyBackgroundMigrator.BeginMigration(new LegacyMigrator());

            await Task.FromResult(true);




            return RedirectToAction("CheckUpgradeProgress");
        }




        [Route("check-upgrade-progress")]
        public ActionResult CheckUpgradeProgress()
        {

            if (LegacyBackgroundMigrator.MigrationTask != null)
            {
                ViewBag.Refresh = "True";
                ViewBag.Messages = LegacyBackgroundMigrator.MessageStack.ToString()
                    .Replace(System.Environment.NewLine, "<br />");
                var upgradeSuccess = false;
                switch (LegacyBackgroundMigrator.MigrationTask.Status)
                {
                    case TaskStatus.Running:
                    case TaskStatus.WaitingForChildrenToComplete:
                    case TaskStatus.WaitingToRun:
                    case TaskStatus.WaitingForActivation:
                        ViewBag.Status = "Running";
                        ViewBag.Alert = "alert-info";
                        break;
                    case TaskStatus.Canceled:
                        ViewBag.Status = "Canceled";
                        ViewBag.Alert = "alert-error";
                        ViewBag.Refresh = "False";
                        LegacyBackgroundMigrator.Reset();
                        break;
                    case TaskStatus.Faulted:
                        ViewBag.Status = "Failed";
                        ViewBag.Alert = "alert-error";
                        ViewBag.Refresh = "False";
                        LegacyBackgroundMigrator.Reset();
                        break;
                    case TaskStatus.RanToCompletion:
                        ViewBag.Status = "Completed";
                        ViewBag.Alert = "alert-success";
                        ViewBag.Refresh = "False";
                        upgradeSuccess = true;
                        LegacyBackgroundMigrator.Reset();
                        break;
                    default:
                        ViewBag.Status = LegacyBackgroundMigrator.MigrationTask.Status.ToString();
                        ViewBag.Alert = "alert-warning";
                        break;
                }

                if (upgradeSuccess)
                {
                    var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
                    if (filter != null)
                    {
                        GlobalFilters.Filters.Remove(filter.Instance);
                    }
                    Database.SetInitializer(new TdIdentityDbInitializer());
                    Database.SetInitializer(new TdPushNotificationDbInitializer());
                    Startup.ConfigurePushNotifications();
                    UpdateSearchIndex();
                }
            }
            return View();
        }

        private void UpdateSearchIndex()
        {
            //TODO: duplicated in DatabaseConfig.cs, should refactor extension method or something... just not sure what the most appropriate place is
            HostingEnvironment.QueueBackgroundWorkItem(async (ct) =>
            {
                using (var ctx = new TdDomainContext(null))
                {
                    await TdSearchContext.Current.IndexManager.RunIndexMaintenanceAsync();
                    var searchItems = ctx.Tickets.Include("TicketEvents").ToSeachIndexItems();
                    await TdSearchContext.Current.IndexManager.AddItemsToIndexAsync(searchItems);
                }
            });
        }

        [HttpPost]
        [Route("create-database")]
        public async Task<ActionResult> CreateDatabase(string email, string password, string displayName)
        {
            using (var ctx = new TdDomainContext(null))
            {
                Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<TdDomainContext, Configuration>(true));
                ctx.Database.Initialize(true);
            }




            var filter = GlobalFilters.Filters.FirstOrDefault(f => f.Instance is DbSetupFilter);
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter.Instance);
            }

            Database.SetInitializer(new TdIdentityDbInitializer());
            var existingUser = UserManager.FindByName(email);
            TicketDeskUser newUser = null;
            if (existingUser == null)
            {
                newUser = new TicketDeskUser { UserName = email, Email = email, DisplayName = displayName };
                await UserManager.CreateAsync(newUser, password);
                await UserManager.AddToRoleAsync(newUser.Id, "TdAdministrators");
               

            }
            else
            {
                //should only happen if user entered one of the demo user names when setting up the DB
                //reset the password to the one the user entered here, and set the admin role if needed
                var token = await UserManager.GeneratePasswordResetTokenAsync(existingUser.Id);
                await UserManager.ResetPasswordAsync(existingUser.Id, token, password);
                existingUser.DisplayName = displayName;
                await UserManager.UpdateAsync(existingUser);
                if (!UserManager.IsTdAdministrator(existingUser.Id))
                {
                    await UserManager.AddToRoleAsync(existingUser.Id, "TdAdministrators");
                }
            }
            Database.SetInitializer(new TdPushNotificationDbInitializer());

            Startup.ConfigurePushNotifications();
            if (newUser != null)
            {
                using (var notificationContext = new TdPushNotificationContext())
                {
                    notificationContext.SubscriberPushNotificationSettingsManager.AddSettingsForSubscriber(
                        new SubscriberNotificationSetting
                        {
                            SubscriberId = newUser.Id,
                            IsEnabled = true,
                            PushNotificationDestinations = new[]
                            {
                                new PushNotificationDestination()
                                {
                                    DestinationType = "email",
                                    DestinationAddress = newUser.Email,
                                    SubscriberName = newUser.DisplayName
                                }
                            }
                        });
                    notificationContext.SaveChanges();
                }
            }
            UpdateSearchIndex();
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
            return View("_NewDatabase", Model);

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
            return View("_LegacySecurity", Model);
        }

        [ChildActionOnly]
        public ActionResult SetupCompleteInfo()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            if (!Model.DatabaseStatus.IsDatabaseReady || !Model.DatabaseStatus.IsCompatibleWithEfModel || Model.DatabaseStatus.HasLegacySecurityObjects)
            {
                return new EmptyResult();
            }
            return View("_SetupCompleteInfo", Model);
        }
    }



}
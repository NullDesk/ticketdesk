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
using TicketDesk.Domain.Search.AzureSearch;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/application-settings")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class ApplicationSettingsController : Controller
    {
        private TicketDeskContext Context { get; set; }
        private bool IsAzureSearchDetected { get; set; }
        public ApplicationSettingsController(TicketDeskContext context)
        {
            Context = context;
            IsAzureSearchDetected = context.SearchManager.GetConnectorType() == typeof(AzureSearchConector) ;

        }

        public ActionResult Index()
        {
            var dbSetting = Context.ApplicationSettings.First(s => s.ApplicationName == "TicketDesk");
            ViewBag.IsAzureSearchEnabled = IsAzureSearchDetected;
            return View(dbSetting);
        }

        [HttpPost]
        public ActionResult Index(
            ApplicationSetting settings,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] categories,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] tickettypes,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] priorities
            )
        {
            var dbSetting = Context.ApplicationSettings.First(s => s.ApplicationName == "TicketDesk");
            if (ModelState.IsValid && TryUpdateModel(dbSetting))
            {
                dbSetting.SelectLists.CategoryList = categories.ToList();
                dbSetting.SelectLists.PriorityList = priorities.ToList();
                dbSetting.SelectLists.TicketTypesList = tickettypes.ToList();
                Context.SaveChanges();
            }
            ViewBag.IsAzureSearchEnabled = IsAzureSearchDetected;
            return View(dbSetting);
        }
    }
}
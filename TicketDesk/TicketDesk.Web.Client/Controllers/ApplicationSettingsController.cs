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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Search.Azure;
using TicketDesk.Search.Common;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/application-settings")]
    [Route("{action=index}")]
    [TdAuthorizeAttribute(Roles = "TdAdministrators")]
    public class ApplicationSettingsController : Controller
    {
        private TdDomainContext Context { get; set; }
        private bool IsAzureSearchDetected { get; set; }
        public ApplicationSettingsController(TdDomainContext context)
        {
            Context = context;
            IsAzureSearchDetected = TdSearchContext.Current.IndexManager.GetType() == typeof(AzureIndexProvider);
        }

        public ActionResult Index()
        {
            var dbSetting = Context.TicketDeskSettings;
            ViewBag.IsAzureSearchEnabled = IsAzureSearchDetected;
            return View(dbSetting);
        }

        [HttpPost]
        public ActionResult Index(
            ApplicationSetting settings,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] categories,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] tickettypes,
            [ModelBinder(typeof(CommaSeparatedModelBinder))] string[] priorities,
            List<string> defaultroles,
            string defaultTextEditorType
            )
        {
            var dbSetting = Context.TicketDeskSettings;
            if (ModelState.IsValid && TryUpdateModel(dbSetting))
            {
                dbSetting.SelectLists.CategoryList = categories.ToList();
                dbSetting.SelectLists.PriorityList = priorities.ToList();
                dbSetting.SelectLists.TicketTypesList = tickettypes.ToList();
                dbSetting.SecuritySettings.DefaultNewUserRoles = defaultroles;
                dbSetting.ClientSettings.Settings["DefaultTextEditorType"] = defaultTextEditorType;
                Context.SaveChanges();
            }
            ViewBag.IsAzureSearchEnabled = IsAzureSearchDetected;
            return View(dbSetting);
        }
    }
}
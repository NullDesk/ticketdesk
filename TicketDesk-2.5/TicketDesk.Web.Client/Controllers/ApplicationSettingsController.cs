using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("admin/application-settings")]
    [Route("{action=index}")]
    [Authorize(Roles = "TdAdministrators")]
    public class ApplicationSettingsController : Controller
    {
        private TicketDeskContext Context { get; set; }
        public ApplicationSettingsController(TicketDeskContext context)
        {
            Context = context;
        }

        public ActionResult Index()
        {
            var dbSetting = Context.ApplicationSettings.First(s => s.ApplicationName == "TicketDesk");

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
            return View(dbSetting);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Controllers
{
    [HandleError]
    [Export("TicketManager", typeof(IController))]
    public partial class TicketManagerController : ApplicationController
    {
        private ITicketService Tickets { get; set; }
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public TicketManagerController(ITicketService ticketService, ISecurityService security, SettingsService settings)
            : base(security)
        {
            Tickets = ticketService;
            Settings = settings;
        }

        [Authorize]
        public virtual ActionResult Index(int? page, string listName)
        {
            var dp = Settings.UserSettings.GetDisplayPreferences();
            int p = page ?? 1;
            if (string.IsNullOrEmpty(listName))
            {
                var listPreference = dp.TicketCenterListPreferences.OrderBy(ls => ls.ListMenuDisplayOrder).FirstOrDefault();
                if (listPreference != null)
                {
                    listName = listPreference.ListName;
                }
            }
            var lp = dp.GetPreferencesForList(listName);

            if (p < 1) //prevent negative pageIndex, redirect to page 1(index 0);
            {
                return RedirectToAction(MVC.TicketManager.Index(1, listName));
            }
            var model = new TicketCenterListViewModel(listName, Tickets.ListTickets(p, lp), Settings, Security);

            if ( p > 1 && p > model.Tickets.TotalPages)//total pages is 0 when no rows returned, so we only do this when requested page is not page 1.
            {
                return RedirectToAction(MVC.TicketManager.Index(model.Tickets.TotalPages, listName));
            }
            if ((TempData["IsRedirectFromAjax"] != null && (bool)TempData["IsRedirectFromAjax"] == true) || this.Request.IsAjaxRequest())
            {

                //return PartialView(MVC.TicketCenter.Views.Controls.TicketList, model);
            }

            return View(model);
        }

    }
}

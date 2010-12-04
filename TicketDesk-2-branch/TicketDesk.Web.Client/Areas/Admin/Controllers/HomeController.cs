using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Controllers;

namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    [Export("Admin", typeof(IController))]
    public partial class HomeController : ApplicationController
    {
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public HomeController(ISecurityService security, SettingsService settings)
            : base(security)
        {
            Settings = settings;
        }

        [Authorize]
        public virtual ActionResult Index()
        {

            if (!Security.IsTdAdmin())
            {
                return MVC.Home.Actions.Index();
            }
            return View();
        }

      


    }
}

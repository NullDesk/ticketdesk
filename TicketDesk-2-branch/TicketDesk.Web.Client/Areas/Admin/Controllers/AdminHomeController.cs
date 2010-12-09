using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;
using TicketDesk.Web.Client.Controllers;
using TicketDesk.Web.Client.Helpers;
using System.Configuration;

namespace TicketDesk.Web.Client.Areas.Admin.Controllers
{
    [Export("AdminHome", typeof(IController))]
    [AuthorizeAdminOnly]
    public partial class AdminHomeController : ApplicationController
    {
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public AdminHomeController(ISecurityService security, SettingsService settings)
            : base(security)
        {
            Settings = settings;
        }

       
        public virtual ActionResult Index()
        {
            ViewData.Add("SecurityMode", ConfigurationManager.AppSettings["SecurityMode"]);
            return View();
        }

      


    }
}

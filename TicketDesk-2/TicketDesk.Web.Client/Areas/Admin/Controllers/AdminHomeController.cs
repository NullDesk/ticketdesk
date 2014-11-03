// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

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

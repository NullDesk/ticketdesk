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

namespace TicketDesk.Web.Client.Controllers
{
    [HandleError]
    [Export("Home", typeof(IController))]
    public partial class HomeController : ApplicationController
    {
        [ImportingConstructor]
        public HomeController(ISecurityService security)
            : base(security)
        {
        }

        public virtual ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}

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

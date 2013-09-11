using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TicketDesk.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Domain.Identity;

namespace TicketDesk.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginForm()
        {
            return View();
        }

    }
}
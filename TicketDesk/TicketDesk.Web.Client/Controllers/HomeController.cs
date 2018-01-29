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

using System;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Localization;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("")]
    [Route("{action=index}")]
    public class HomeController : Controller
    {
        [Route("language")]
        public ActionResult SetLanguage(string name)
        {
            CultureHelper.SetCurrentCulture(name);

            var cookie = new HttpCookie("_culture", name);
            cookie.Expires = DateTime.Today.AddYears(1);
            Response.SetCookie(cookie);

            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.AbsoluteUri))
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            else
                return RedirectToAction("Index", "Home");
        }

        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            if (ApplicationConfig.HomeEnabled)
            {
                return View();
            }
            else
            {
                return RedirectToActionPermanent("Index", "TicketCenter");
            }
        }
 
        [Route("about")]
        public ActionResult About()
        {
            return View();
        }

        [Route("access-denied", Name = "AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace TicketDesk.Web.Controllers
{
    /// <summary>
    /// This Controller contains the Index action where all request will be redirected on this site
    /// </summary>
    public class HomeController : AsyncController
    {
        public async Task<ActionResult> Index()
        {
            return View();

        }

    }
}


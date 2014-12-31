using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("")]
    [Route("{action=index}")]
    public class HomeController : Controller
    {
        private TicketDeskContext Context { get; set; }

        public HomeController(TicketDeskContext context)
        {
            Context = context;
        }

        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View();
        }
 
        [Route("about")]
        public ActionResult About()
        {
            return View();
        }
    }
}
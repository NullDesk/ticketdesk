using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    [Authorize]
    public class TicketActivityController : Controller
    {
        
        public ActionResult AddComment()
        {
            return PartialView("_AddComment",new TicketComment());
        }

        [HttpPost]
        public ActionResult AddComment(TicketComment comment)
        {
            return new EmptyResult();
        }
    }
}
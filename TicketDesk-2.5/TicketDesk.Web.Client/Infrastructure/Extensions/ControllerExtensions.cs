using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public static class ControllerExtensions
    {
        public static bool IsItReallyRedirectFromAjax(this Controller controller)
        {
            return ((controller.TempData["IsRedirectFromAjax"] != null && (bool)controller.TempData["IsRedirectFromAjax"] == true) || controller.Request.IsAjaxRequest());
        }
    }
}
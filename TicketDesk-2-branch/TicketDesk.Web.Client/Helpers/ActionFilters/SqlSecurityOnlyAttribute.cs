using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Routing;

namespace TicketDesk.Web.Client.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlSecurityOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.Equals(ConfigurationManager.AppSettings["SecurityMode"], "SQL"))
            {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {Area = "Admin", Controller = "AdminHome", Action = "Index"}));
                return;
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
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
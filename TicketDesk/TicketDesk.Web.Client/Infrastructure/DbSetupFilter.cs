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

using System.Web.Mvc;
using System.Web.Routing;

namespace TicketDesk.Web.Client
{
    public class DbSetupFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //we can only get here if the DB needs attention

            var ctlr = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //only allowed to go to data management or login
            if (ctlr != "FirstRunSetup")
            {
                filterContext.Result = new RedirectToRouteResult("first-run-setup",
                    new RouteValueDictionary(
                        new
                        {
                            Action = "Index",
                            Controller = "FirstRunSetup",
                        }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}

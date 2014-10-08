using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using TicketDesk.Web.Client.Areas.Initialization.Controllers;

namespace TicketDesk.Web.Client
{
    public class DbSetupFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //we can only get here if the DB needs attention

            var ctlr = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //only allowed to go to data management or login
            if (ctlr != "DataManagement")
            {

                var action = DatabaseConfig.IsLegacyDatabase() ? "Upgrade" : "Create";

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            action,
                            controller = "DataManagement",
                            area = "Initialization"
                        }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}

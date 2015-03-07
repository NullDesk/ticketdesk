using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace TicketDesk.Web.Client
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class WhenSetupEnabledAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var setupEnabled = ConfigurationManager.AppSettings["ticketdesk:SetupEnabled"];
            var firstRunEnabled = !string.IsNullOrEmpty(setupEnabled) && setupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            if (!firstRunEnabled)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            Action = "Index",
                            Controller = "Home"
                        }));
            }
        }
    }
}
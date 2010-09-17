using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Helpers
{
    /// <summary>
    /// Allows partial model validation
    /// </summary>
    /// <remarks>
    /// from: http://blog.stevensanderson.com/2010/02/19/partial-validation-in-aspnet-mvc-2/
    /// </remarks>
    public class ValidateOnlyIncomingValuesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            var valueProvider = filterContext.Controller.ValueProvider;

            var keysWithNoIncomingValue = modelState.Keys.Where(x => !valueProvider.ContainsPrefix(x));
            foreach (var key in keysWithNoIncomingValue)
                modelState[key].Errors.Clear();
        }
        
    }
}
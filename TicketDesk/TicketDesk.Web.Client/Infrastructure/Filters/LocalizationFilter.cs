using System.Web;
using System.Web.Mvc;
using TicketDesk.Localization;

namespace TicketDesk.Web.Client.Infrastructure.Filters
{
    public class LocalizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = HttpContext.Current.Request.Cookies["_culture"];
            var name = cookie != null ? cookie.Value : null;

            if (string.IsNullOrEmpty(name))
            {
                CultureHelper.SetDefaultCulture();
                return;
            }

            CultureHelper.SetCurrentCulture(name);
        }
    }
}
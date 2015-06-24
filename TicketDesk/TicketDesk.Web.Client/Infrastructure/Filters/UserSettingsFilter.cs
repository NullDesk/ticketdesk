using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client
{
    public class UserSettingsFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var ident = HttpContext.Current.User.Identity;
            if (ident.IsAuthenticated)
            {
                var id = ident.GetUserId();
                var context = DependencyResolver.Current.GetService<TdDomainContext>();
                var settings = AsyncHelper.RunSync(() => context.UserSettingsManager.GetSettingsForUserAsync(id));
                filterContext.Controller.ViewBag.SelectedProjectId = settings.SelectedProjectId;
            }

        }
    }
}
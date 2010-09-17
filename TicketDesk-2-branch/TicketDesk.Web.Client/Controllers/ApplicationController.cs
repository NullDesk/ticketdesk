using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using System.Web.Security;
using TicketDesk.Domain.Services;
using System.Linq.Expressions;

namespace TicketDesk.Web.Client.Controllers
{

    public abstract partial class ApplicationController : Controller
    {
        public ISecurityService Security { get; set; }

        public ApplicationController(ISecurityService security)
        {
            Security = security;
        }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserDisplayName"] = Security.GetUserDisplayName();
            }
            base.OnActionExecuting(filterContext);
        }



        protected bool IsItReallyRedirectFromAjax()
        {
            return ((TempData["IsRedirectFromAjax"] != null && (bool)TempData["IsRedirectFromAjax"] == true) || this.Request.IsAjaxRequest());
        }

     

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Web.Client.Controllers;

namespace TicketDesk.Web.Client.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAdminOnly : AuthorizeAttribute
    {
        private ApplicationController controller;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAdmin = (controller != null && controller.Security != null && controller.Security.IsTdAdmin());
            
            return isAdmin ? base.AuthorizeCore(httpContext) : false;


        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            controller = filterContext.Controller as ApplicationController;

            base.OnAuthorization(filterContext);
        }
    }
}
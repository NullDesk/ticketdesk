using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using System.Web.Security;
using TicketDesk.Domain.Services;
using System.Linq.Expressions;
using System.Web.Routing;
using System.IO;

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
                
                    //poor man's session state, put this stuff back in tempdata each time.
                    //  Only doing this to prevent spinning up the whole session state 
                    //  mechanism... basically just doing it "the MVC way", sorta.
                    TempData.Keep("TicketCenterList");
                    TempData.Keep("TicketCenterPage");
                    TempData.Keep("TicketCenterListDisplayName");
                
            }
            base.OnActionExecuting(filterContext);
        }



        protected bool IsItReallyRedirectFromAjax()
        {
            return ((TempData["IsRedirectFromAjax"] != null && (bool)TempData["IsRedirectFromAjax"] == true) || this.Request.IsAjaxRequest());
        }



        public RouteData GetReferringRoute()
        {
            using (StringWriter sw = new StringWriter())
            {
                var url = this.Request.UrlReferrer.AbsolutePath;
                var querystring = this.Request.UrlReferrer.Query;
                var fakeResponse = new HttpResponse(sw);
                var fakeContext = new HttpContext(new HttpRequest("", url, querystring), fakeResponse);
                var route = RouteTable.Routes.GetRouteData(new HttpContextWrapper(fakeContext));
                return route;
            }
        }

    }
}

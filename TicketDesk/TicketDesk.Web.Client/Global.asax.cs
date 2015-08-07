using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using TicketDesk.Localization;

namespace TicketDesk.Web.Client
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
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

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
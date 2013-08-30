using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

namespace TicketDesk.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
            app.Use<WebApiAuthenticationRedirectHackWare>();
        }
    }
    public class WebApiAuthenticationRedirectHackWare : OwinMiddleware
    {
        //TODO: This is (probably) only necessary because we are using aspnet identity with web api v1.
        /*
         * Normally, web api suppresses 302 redirects when authentication fails, which is working. But,
         *      it appears that the cookie auth stuff in asp.net identity does its own authentication 
         *      redirects -- converting 401 "unauthorized" responses into 302 "go login" ones --
         *      and it doesn't make an exception for web api requests vs. regular mvc requests.
         *   
         * I suspect that the reason is that you are expected, in web api v2, to setup 
         *      UseOAuthBearerAuthentication with UseExternalSignInCookie (in startup.auth.cs) instead.
         *      This bearer stuff is also setup in webapi's global config as a HostAuthenticationFilter,
         *      which I suspect probably handles suppressing redirects (or just doesn't do them). If not,
         *      then hopefully some other part of the vNext owin and aspnet identity stuff will be designed
         *      to deal with the problem.
         *      
         * Since we're using the bleeding edge with the older v1 web api platform though, this hack manually
         *      detects 302's coming from web api mapped urls, and converts them back to 401's. 
         *      
         * The whole point is that ajax needs to get 401 instead of being redirected to an html resouece that
         *      responds with a 200 and real live content.
         *  
         */
        public WebApiAuthenticationRedirectHackWare(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {

            context.Response.OnSendingHeaders(state =>
            {
               
                var resp = (Microsoft.Owin.OwinResponse)state;
                if (context.Request.Path.Contains("/breeze/") || context.Request.Path.Contains("/api/"))
                {
                    if (resp.StatusCode == 302)
                    {
                        resp.StatusCode = 401;
                        resp.Headers.Remove("Location");
                    }
                }

                //resp.SetHeader("X-MyResponse-Header", "Some Value");
                //resp.StatusCode = 403;
            }, context.Response);
            //var header = request.GetHeader("X-Whatever-Header");

            await Next.Invoke(context);

            //response.SetHeader("X-MyResponse-Header", "Some Value");
            //response.StatusCode = 403;

        }
    }

}

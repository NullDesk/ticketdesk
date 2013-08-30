using Owin;
using Microsoft.Owin.Security.Cookies;

namespace TicketDesk.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider

            //var options = new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions() { LoginPath = "~/#/login" };
            //Microsoft.Owin.Security.Cookies.
            var o = new Microsoft.AspNet.Identity.Owin.IdentityAuthenticationOptions();
            o.LoginPath = "/#/login";
         
            app.UseSignInCookies(o);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication();
        }
    }
}
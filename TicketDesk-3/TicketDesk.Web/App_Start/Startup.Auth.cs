using System.Configuration;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using TicketDesk.Domain.Identity;
using TicketDesk.Web.Providers;
using System;
using TicketDesk.Domain.Model;
using TicketDesk.Domain;

namespace TicketDesk.Web
{
    public partial class Startup
    {	
		//Enable OWIN Bearer Token Middleware	
        static Startup()
        {			
            PublicClientId = "self";

            UserManagerFactory = () => new TicketDeskUserManager();
            //new UserManager<UserProfile>(new UserStore<UserProfile>(new TicketDeskContext()));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
				//Exposes Token endpoint
                TokenEndpointPath = new PathString("/Token"),
				//Use ApplicationOAuthProvider in order to authenticate
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14), //Token expiration => The user will remain authenticated for 14 days
                AllowInsecureHttp = true                
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static System.Func<UserManager<UserProfile>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Enable the application to use bearer tokens to authenticate users
			// Enabling 3 components:
			// 1. Authorization Server middleware. For creating the bearer tokens
			// 2. Application bearer token middleware. Will atuthenticate every request with Authorization : Bearer header
			// 3. External bearer token middleware. For external providers
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseMicrosoftAccountAuthentication(ConfigurationManager.AppSettings["MicrosoftKey"], ConfigurationManager.AppSettings["MicrosoftSecret"]);

            app.UseTwitterAuthentication(ConfigurationManager.AppSettings["TwitterKey"], ConfigurationManager.AppSettings["TwitterSecret"]);

            app.UseFacebookAuthentication(ConfigurationManager.AppSettings["FacebookKey"], ConfigurationManager.AppSettings["FacebookSecret"]);

            app.UseGoogleAuthentication();       
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            IReadableStringCollection query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SimpleInjector;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Infrastructure;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app, Container container)
        {


           
            //non-IoC stuff... no longer needed, but here for reference.
            //app.CreatePerOwinContext(TicketDeskIdentityContext.Create);
            //app.CreatePerOwinContext<TicketDeskUserManager>(TicketDeskUserManager.Create);
            //app.CreatePerOwinContext<TicketDeskRoleManager>(TicketDeskRoleManager.Create);
            //app.CreatePerOwinContext<TicketDeskSignInManager>(TicketDeskSignInManager.Create);
            
            
            app.CreatePerOwinContext<TicketDeskUserManager>(container.GetInstance<TicketDeskUserManager>);
            
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<TicketDeskUserManager, TicketDeskUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

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

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            var demoRefresh = ConfigurationManager.AppSettings["ticketdesk:ResetDemoDataOnStartup"];
            var firstRunDemoRefresh = !string.IsNullOrEmpty(demoRefresh) &&
                demoRefresh.Equals("true", StringComparison.InvariantCultureIgnoreCase) &&
                DatabaseConfig.IsDatabaseReady;//only do this if database was ready on startup, otherwise migrator will take care of it

            if (firstRunDemoRefresh)
            {
                DemoIdentityDataManager.SetupDemoIdentityData(container.GetInstance<TicketDeskIdentityContext>());
            }

        }
    }
}
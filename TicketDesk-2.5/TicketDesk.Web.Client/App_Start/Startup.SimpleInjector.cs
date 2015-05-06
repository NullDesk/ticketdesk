// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.PushNotifications.Common;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public Container RegisterStructureMap(IAppBuilder app)
        {
            var container = GetInitializedContainer(app);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public Container GetInitializedContainer(IAppBuilder app)
        {
            var container = new Container();

            container.RegisterSingle(app);


            //allows objects to be reused when inside web request, or created fresh when used on background threads or outside a request context
            var hybridLifestyle = Lifestyle.CreateHybrid(
                () => HttpContext.Current != null, new WebRequestLifestyle(), Lifestyle.Transient);

            container.RegisterPerWebRequest<TicketDeskContextSecurityProvider>();

            container.Register(() => new TdPushNotificationContext(), hybridLifestyle);

            container.Register(() => HttpContext.Current != null ?
                    new TdDomainContext(container.GetInstance<TicketDeskContextSecurityProvider>()) :
                    new TdDomainContext(),
                hybridLifestyle);

            container.RegisterPerWebRequest<TdIdentityContext>();

            container.RegisterPerWebRequest<IUserStore<TicketDeskUser>>(() =>
                new UserStore<TicketDeskUser>(container.GetInstance<TdIdentityContext>()));

            container.RegisterPerWebRequest<IRoleStore<IdentityRole, string>>(() =>
                new RoleStore<IdentityRole>(container.GetInstance<TdIdentityContext>()));


            container.RegisterPerWebRequest(() =>
            {
                IOwinContext context;
                try
                {
                    context = HttpContext.Current.GetOwinContext();
                }
                catch (InvalidOperationException)
                {
                    //avoid exception when this is called before the owin environment is fully initialized
                    if (container.IsVerifying())
                    {
                        return new FakeAuthenticationManager();
                    }
                    throw;
                }

                return context.Authentication;
            }
                );

            container.RegisterPerWebRequest<SignInManager<TicketDeskUser, string>, TicketDeskSignInManager>();

            container.RegisterPerWebRequest<TicketDeskRoleManager>();

            container.RegisterInitializer<TicketDeskUserManager>(manager =>
                InitializeUserManager(manager, app));


            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            return container;
        }



        private void InitializeUserManager(TicketDeskUserManager manager, IAppBuilder app)
        {
            manager.UserValidator = new UserValidator<TicketDeskUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<TicketDeskUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<TicketDeskUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            //TODO: research DpapiDataProtectionProvider and figure out what the f*** this is supposed to do
            var dataProtectionProvider = app.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<TicketDeskUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        class FakeAuthenticationManager : IAuthenticationManager
        {
            //see this: https://simpleinjector.codeplex.com/discussions/564822
            public AuthenticationResponseChallenge AuthenticationResponseChallenge { get; set; }
            public AuthenticationResponseGrant AuthenticationResponseGrant { get; set; }
            public AuthenticationResponseRevoke AuthenticationResponseRevoke { get; set; }
            public ClaimsPrincipal User { get; set; }

            public Task<IEnumerable<AuthenticateResult>> AuthenticateAsync(string[] authenticationTypes)
            {
                throw new NotImplementedException();
            }

            public Task<AuthenticateResult> AuthenticateAsync(string authenticationType)
            {
                throw new NotImplementedException();
            }

            public void Challenge(params string[] authenticationTypes)
            {
                throw new NotImplementedException();
            }

            public void Challenge(AuthenticationProperties properties, params string[] authenticationTypes)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<AuthenticationDescription> GetAuthenticationTypes(
                Func<AuthenticationDescription, bool> predicate)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<AuthenticationDescription> GetAuthenticationTypes()
            {
                throw new NotImplementedException();
            }

            public void SignIn(params ClaimsIdentity[] identities) { }
            public void SignIn(AuthenticationProperties properties, params ClaimsIdentity[] identities) { }
            public void SignOut(params string[] authenticationTypes) { }
            public void SignOut(AuthenticationProperties properties, params string[] authenticationTypes) { }
        }
    }
}
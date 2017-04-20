// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.PushNotifications;
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
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterSingleton(app);

            
            //allows objects to be reused when inside web request, or created fresh when used on background threads or outside a request context
            var hybridLifestyle = Lifestyle.CreateHybrid(
                () => HttpContext.Current != null, new WebRequestLifestyle(), Lifestyle.Transient);

            container.Register<TicketDeskContextSecurityProvider>(Lifestyle.Scoped);


            container.Register(() => HttpContext.Current != null ?
                    new TdDomainContext(container.GetInstance<TicketDeskContextSecurityProvider>()) :
                    new TdDomainContext(),
                hybridLifestyle);

            container.Register(() => new TdIdentityContext(), hybridLifestyle);

            container.Register<IUserStore<TicketDeskUser>>(() =>
                new UserStore<TicketDeskUser>(container.GetInstance<TdIdentityContext>()), 
                hybridLifestyle);

            container.Register<IRoleStore<TicketDeskRole, string>>(() =>
                new RoleStore<TicketDeskRole>(container.GetInstance<TdIdentityContext>()), 
                hybridLifestyle);


            container.Register(() =>
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
            },
            Lifestyle.Scoped);

            container.Register<SignInManager<TicketDeskUser, string>, TicketDeskSignInManager>(Lifestyle.Scoped);

            container.Register<TicketDeskRoleManager>(hybridLifestyle);

            container.RegisterInitializer<TicketDeskUserManager>(manager =>
                manager.ConfigureDataProtection(app));


            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            return container;
        }
    }
}
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

using System.Globalization;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using TicketDesk.Web.Client;

[assembly: OwinStartup(typeof(Startup))]
namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
            var container = RegisterStructureMap(app);
            ConfigureDatabase();
            ConfigureSearch();
            ConfigureAuth(app, container);
            ConfigurePushNotifications();
            ConfigureApplicationInsights();

            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}

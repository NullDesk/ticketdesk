using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using ngWebClientAPI;
using System.Web.Mvc;
using System.Web.Optimization;


[assembly: OwinStartup(typeof(ngWebClientAPI.Startup))]

namespace ngWebClientAPI
{
    [System.Web.Http.Authorize]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
    
            RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);
            //RegisterBundles(BundleTable.Bundles);

            var container = RegisterStructureMap(app);
            ConfigureDatabase();
            ConfigureSearch();
            GlobalConfig.categories = null;
            GlobalConfig.priorities = null;
            GlobalConfig.ticketTypes = null;
            GlobalConfig.SLASettings = null;
            //ConfigureAuth(app, container);
            //ConfigurePushNotifications();
            //ConfigureApplicationInsights();
            //WatchdogThreads threading = new WatchdogThreads(new TicketDesk.Domain.TdDomainContext());
            //WatchdogThreads.StartWatch();
        }
    }
}

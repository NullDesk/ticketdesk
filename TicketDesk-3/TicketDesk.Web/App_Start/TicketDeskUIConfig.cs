using System;
using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(
    typeof(TicketDesk.Web.App_Start.TicketDeskUIConfig), "PreStart")]

namespace TicketDesk.Web.App_Start
{
    public static class TicketDeskUIConfig
    {
        public static void PreStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
using System;
using System.Web.Optimization;

[assembly: WebActivator.PostApplicationStartMethod(typeof(TicketDesk.Web.TicketDeskUIConfig), "PostStart")]
namespace TicketDesk.Web
{
    public static class TicketDeskUIConfig
    {
        public static void PostStart()
        {
            // Add your start logic here
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
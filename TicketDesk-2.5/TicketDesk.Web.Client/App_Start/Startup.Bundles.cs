using System.Web;
using System.Web.Optimization;

namespace TicketDesk.Web.Client
{
    //TODO: This may not be OWIN compliant if OWIN hosts other than Microsoft.Owin.Host.SystemWeb are used
    public partial class Startup
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/ticketcenter").Include(
                "~/Scripts/ticketdesk/ticketcenter.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/markdown").Include(
                        "~/Scripts/pagedown/Markdown.Converter.js",
                        "~/Scripts/pagedown/Markdown.Sanitizer.js",
                        "~/Scripts/pagedown/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.clickable-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Scripts/corner/jquery.corner.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
#if (DEBUG)
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

        }
    }
}

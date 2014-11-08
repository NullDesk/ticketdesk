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

            bundles.Add(new StyleBundle("~/content/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/site.css",
                     "~/Content/font-awesome.css",
                     "~/Content/css/select2.css",
                     "~/Content/css/select2-bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/ticketcenter").Include(
                "~/Scripts/ticketdesk/ticketcenter.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/markdown").Include(
                        "~/Scripts/pagedown/Markdown.Converter.js",
                        "~/Scripts/pagedown/Markdown.Sanitizer.js",
                        "~/Scripts/pagedown/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.clickable-{version}.js",
                        "~/Scripts/jquery.validate*", 
                        "~/Scripts/select2.js",
                        "~/Scripts/bootstrap.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


           

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

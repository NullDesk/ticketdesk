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

            bundles.Add(new StyleBundle("~/content/styles").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css",
                    "~/Content/font-awesome.css"
                    ));
          
            bundles.Add(new StyleBundle("~/content/ticketeditor")
                    .Include("~/Content/css/select2.css", new CssRewriteUrlTransform())
                    .Include("~/Content/css/select2-bootstrap.css", new CssRewriteUrlTransform())
                    .Include("~/Scripts/dropzone/css/dropzone.css", new CssRewriteUrlTransform())
                    .Include("~/Scripts/dropzone/css/basic.css",new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/ticketcenter").Include(
                    "~/Scripts/ticketdesk/ticketcenter.js",
                    "~/Scripts/jquery.clickable-{version}.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/ticketeditor").Include(
                    "~/Scripts/dropzone/dropzone.js",
                    "~/Scripts/select2.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/markdown").Include(
                    "~/Scripts/pagedown/Markdown.Converter.js",
                    "~/Scripts/pagedown/Markdown.Sanitizer.js",
                    "~/Scripts/pagedown/Markdown.Editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/jquery.validate*",
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

using System;
using System.Web.Optimization;

namespace TicketDesk.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            // js Vendor
            bundles.Add(
              new ScriptBundle("~/scripts/vendor")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/knockout-{version}.js")
                .Include("~/Scripts/knockout.validation.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/jquery.hammer.min.js")
                .Include("~/Scripts/Stashy.js")
                .Include("~/Scripts/Q.js")
                .Include("~/Scripts/breeze.min.js")
                .Include("~/Scripts/moment.js")
                .Include("~/Scripts/jquery.tagsinput.js")
                .Include("~/Scripts/marked.js")
                .Include("~/Scripts/zen-form.js")
                .Include("~/Scripts/highlight.pack.js")
                .Include("~/Scripts/jquery.tagsinput.js")
              );
            IItemTransform cssFixer = new CssRewriteUrlTransform();

            // css vendor
            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/Content/ie10mobile.css")
                //.Include("~/content/bootstrap/bootstrap.css", cssFixer)
                //.Include("~/content/bootstrap/bootstrap-theme.css", cssFixer)
                .Include("~/Content/bootstrap.css", cssFixer)
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/durandal.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/Stashy.css")
                .Include("~/Content/jquery.tagsinput.css")
                .Include("~/Content/zen-form.css")
                .Include("~/Content/vs.css")
                .Include("~/Content/social-buttons-3.css")
              );

            // css custom
            bundles.Add(
              new StyleBundle("~/Content/custom")
                .Include("~/Content/app.css")
              );
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
            {
                throw new ArgumentNullException("ignoreList");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}
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

            bundles.Add(new ScriptBundle("~/scripts/modernizr")
                .Include("~/scripts/modernizr-{version}.js")
            );

            bundles.Add(
              new ScriptBundle("~/scripts/vendor")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/knockout-{version}.js")
                .Include("~/scripts/sammy-{version}.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/scripts/breeze.min.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/moment.js")
                .Include("~/Scripts/i18next-{version}.js")
                .Include("~/scripts/pagedown/Markdown.Converter.js")
                .Include("~/scripts/pagedown/Markdown.Editor.js")
                .Include("~/scripts/pagedown/Markdown.Sanitizer.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/signalr")
                .Include("~/Scripts/jquery.signalR-{version}.js")
            );
            IItemTransform cssFixer = new CssRewriteUrlTransform();

            bundles.Add(
              new StyleBundle("~/content/css")
                .Include("~/content/ie10mobile.css")
                .Include("~/content/bootstrap/bootstrap.css", cssFixer)
                .Include("~/content/bootstrap/bootstrap-theme.css", cssFixer)
                .Include("~/content/font-awesome.css")
                .Include("~/content/durandal.css")
                .Include("~/content/toastr.css")
                .Include("~/content/app.css")
                .Include("~/content/pagedown/pagedown.bootstrap.fontawesome.css")
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

            //ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}
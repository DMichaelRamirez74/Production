using System.Web;
using System.Web.Optimization;

namespace Fingerprints
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/weeklyattendance").Include("~/Content/bootstrap-3.3.6-dist/css/bootstrap.css").
                Include("~/Content/css/weekly-attendance.css").Include("~/Content/testcss/jquery.datetimepicker.css").
                Include("~/Content/themes/base/jquery-ui.css").
                Include("~/Scripts/jquery.signature.css"));

            bundles.Add(new ScriptBundle("~/bundles/weeklyattendance").Include("~/Scripts/OfflineStorage/jquery-{version}.js").
                //Include("~/Scripts/Samplescript/jquery-ui.min.js").
                Include(
                      "~/Scripts/OfflineStorage/jquery.blockUI.js",
                      "~/Scripts/OfflineStorage/cache.js"
                      ,"~/Content/bootstrap-3.3.6-dist/js/bootstrap.min.js",
                       "~/Scripts/OfflineStorage/weeklyattendance.js",
                     "~/Scripts/OfflineStorage/bootbox.min.js",
                      "~/Scripts/OfflineStorage/Config.js","~/Scripts/OfflineStorage/db.js","~/Scripts/OfflineStorage/DbManager.js", "~/Scripts/Samplescript/jqueryui.js",
                      "~/Scripts/jquery.signature.js"));
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include("~/Scripts/Samplescript/jquery.datetimepicker.full.min.js"));
        }
    }
}
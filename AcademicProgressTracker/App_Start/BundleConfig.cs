using System.Web;
using System.Web.Optimization;

namespace AcademicProgressTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Content/Scripts/jquery-{version}.js",
                        "~/Content/Scripts/jquery-ui-1.12.1.js",
                        "~/Content/Scripts/bootstrap.js",
                        "~/Content/Scripts/bootbox.js",
                        "~/Content/Scripts/respond.js",
                        "~/Content/Scripts/toastr.js",
                        "~/Content/Scripts/canvasjs.min.js",
                        "~/Content/Scripts/jquery.canvasjs.min.js",
                        "~/Content/Scripts/datatables/jquery.datatables.js",
                        "~/Content/Scripts/datatables/datatables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-cerulean.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css",
                      "~/Content/themes/base/jquery-ui.css"));
        }
    }
}

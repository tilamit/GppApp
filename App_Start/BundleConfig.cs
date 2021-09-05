using System.Web;
using System.Web.Optimization;

namespace GppApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/js/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                       "~/assets/vendors/mdi/css/materialdesignicons.min.css",
                       "~/assets/vendors/css/vendor.bundle.base.css",
                       "~/assets/vendors/jvectormap/jquery-jvectormap.css",
                       "~/assets/vendors/flag-icon-css/css/flag-icon.min.css",
                       "~/assets/vendors/owl-carousel-2/owl.carousel.min.css",
                       "~/assets/vendors/owl-carousel-2/owl.theme.default.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/assets/css/style.css",
                      "~/assets/css/custom.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/vendors/js/vendor.bundle.base.js",
                        "~/assets/vendors/chart.js/Chart.min.js",
                        "~/assets/vendors/progressbar.js/progressbar.min.js",
                        "~/assets/vendors/jvectormap/jquery-jvectormap.min.js",
                        "~/assets/vendors/jvectormap/jquery-jvectormap-world-mill-en.js",
                        "~/assets/vendors/owl-carousel-2/owl.carousel.min.js",
                        "~/assets/js/off-canvas.js",
                        "~/assets/js/hoverable-collapse.js",
                        "~/assets/js/misc.js",
                        "~/assets/js/settings.js",
                        "~/assets/js/todolist.js",
                        "~/assets/js/dashboard.js"
                        ));

               
            #if DEBUG
               BundleTable.EnableOptimizations = false;
            #else
               BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}

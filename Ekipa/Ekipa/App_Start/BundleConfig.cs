using System.Web;
using System.Web.Optimization;

namespace Ekipa
{

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/slick-slider").Include(
                    "~/Scripts/slick.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/slick-slider-style").Include(
                    "~/Content/style/slick-theme.css"));

            bundles.Add(new StyleBundle("~/Content/style").Include(
                    "~/Content/style/StyleSheet.css"
                    ));
        }
    }
}

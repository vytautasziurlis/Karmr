using System.Web.Optimization;

namespace Karmr.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region OldTheme

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/old/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/old/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/old/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/old/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/cssOld").Include(
                "~/Content/old/bootstrap.css",
                "~/Content/old/site.css"));

            #endregion

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery.js",
                "~/Scripts/jquery.collapsible.min.js",
                "~/Scripts/swiper.min.js",
                "~/Scripts/jquery.countdown.min.js",
                "~/Scripts/circle-progress.min.js",
                "~/Scripts/jquery.countTo.min.js",
                "~/Scripts/jquery.barfiller.js",
                "~/Scripts/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/elegant-fonts.css",
                "~/Content/themify-icons.css",
                "~/Content/swiper.min.css",
                "~/Content/style.css"));
        }
    }
}

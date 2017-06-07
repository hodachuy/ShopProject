using Shopproject.Common;
using System.Web;
using System.Web.Optimization;

namespace ShopProject.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/assets/client/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/js/plugins").Include(
                 "~/assets/admin/libs/jquery-ui/jquery-ui.min.js",
                 "~/assets/admin/libs/mustache/mustache.js",
                 "~/assets/admin/libs/numeral/numeral.js",
                 "~/assets/admin/libs/iscroll/build/iscroll.js",
                 "~/assets/admin/libs/drawer/dist/js/drawer.js",
                 "~/assets/admin/libs/jquery-validation/dist/jquery.validate.js",
                 "~/assets/client/js/controller/shoppingCart.js",
                 "~/assets/client/js/Common.js",
                 "~/assets/admin/libs/toastr/toastr.js"
                ));

            bundles.Add(new StyleBundle("~/css/base")
                .Include("~/assets/client/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/font-awesome-4.6.3/css/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/css/style.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/css/custom.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/libs/jquery-ui/themes/smoothness/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/css/etalage.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/libs/toastr/toastr.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/libs/drawer/dist/css/drawer.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/css/cartTemplate.css", new CssRewriteUrlTransform())
                .Include("~/assets/client/menudropdown/css/styleMenu.css", new CssRewriteUrlTransform())
                );
            BundleTable.EnableOptimizations = bool.Parse(ConfigHelper.GetByKey("EnableBundles"));
        }
    }
}

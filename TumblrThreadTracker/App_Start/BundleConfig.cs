using System.Web.Optimization;
using BundleTransformer.Core.Bundles;

namespace TumblrThreadTracker
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new CustomScriptBundle("~/bundles/angular-bootstrap").Include("~/application/scripts/app.js"));
            bundles.Add(new CustomScriptBundle("~/bundles/vendor").Include("~/application/scripts/vendor/*.js"));
            bundles.Add(
                new CustomScriptBundle("~/bundles/angular-controllers").Include("~/application/scripts/controllers/*.js"));
            bundles.Add(
                new CustomScriptBundle("~/bundles/angular-directives").Include("~/application/scripts/directives/*.js"));
            bundles.Add(new CustomScriptBundle("~/bundles/angular-filters").Include("~/application/scripts/filters/*.js"));
            bundles.Add(
                new CustomScriptBundle("~/bundles/angular-services").Include("~/application/scripts/services/*.js"));


            //BundleTable.EnableOptimizations = true;
        }
    }
}
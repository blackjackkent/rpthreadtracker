using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Bundles;

namespace TumblrThreadTracker
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new CustomScriptBundle("~/bundles/angular-bootstrap").Include(
                 "~/application/scripts/app.js"));
            bundles.Add(new CustomScriptBundle("~/bundles/angular-application")
                .Include("~/application/scripts/directives.js")
                .Include("~/application/scripts/controllers.js")
                .Include("~/application/scripts/filters.js")
                .Include("~/application/scripts/services.js"));


            BundleTable.EnableOptimizations = true;
        }
    }
}
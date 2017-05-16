namespace RPThreadTracker
{
	using System.Web.Optimization;
	using BundleTransformer.Core.Bundles;
	using Infrastructure.Filters;

	/// <summary>
	/// Class defining setup of script bundles
	/// </summary>
	[ExcludeFromCoverage]
	public class BundleConfig
	{
		/// <summary>
		/// Initialize script bundles to be loaded into MVC view Index.cshtml
		/// </summary>
		/// <remarks>
		/// These bundles are unoptimized in debug mode by default.
		/// This is controlled by the <c>compilation</c> tag in web.config.
		/// Use <c>BundleTable.EnableOptimizations = true</c> to temporarily
		/// enable optimization in debug mode.
		/// </remarks>
		/// <param name="bundles">Default BundleCollection object from WebAPI</param>
		public static void RegisterBundles(BundleCollection bundles)
		{
			// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
			bundles.Add(new CustomScriptBundle("~/bundles/bower").Include(
				"~/application/scripts/bower/angular/angular.min.js",
				"~/application/scripts/bower/angular-route/angular-route.min.js",
				"~/application/scripts/bower/angulartics/dist/angulartics.min.js",
				"~/application/scripts/bower/angulartics-google-analytics/dist/angulartics-ga.min.js",
				"~/application/scripts/bower/lodash/dist/lodash.min.js",
				"~/application/scripts/bower/bootstrap-switch/dist/js/bootstrap-switch.min.js",
				"~/application/scripts/bower/angular-bootstrap-switch/dist/angular-bootstrap-switch.min.js",
				"~/application/scripts/bower/angular-ui-notification/dist/angular-ui-notification.min.js",
				"~/application/scripts/bower/angular-aria/angular-aria.min.js",
				"~/application/scripts/bower/angular-sanitize/angular-sanitize.min.js",
				"~/application/scripts/bower/angular-animate/angular-animate.min.js",
				"~/application/scripts/bower/angular-material/angular-material.min.js"));
			bundles.Add(new CustomScriptBundle("~/bundles/angular-bootstrap").Include("~/application/scripts/app.js"));
			bundles.Add(new CustomScriptBundle("~/bundles/angular-controllers").Include("~/application/scripts/controllers/*.js"));
			bundles.Add(new CustomScriptBundle("~/bundles/angular-directives").Include("~/application/scripts/directives/*.js"));
			bundles.Add(new CustomScriptBundle("~/bundles/angular-filters").Include("~/application/scripts/filters/*.js"));
			bundles.Add(new CustomScriptBundle("~/bundles/angular-services").Include("~/application/scripts/services/*.js"));

			// BundleTable.EnableOptimizations = true;
		}
	}
}
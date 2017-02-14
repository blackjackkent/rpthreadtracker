namespace RPThreadTracker
{
	using System.Web.Mvc;

	/// <summary>
	/// Class defining setup of WebAPI Filters
	/// </summary>
	public class FilterConfig
	{
		/// <summary>
		/// Register any filters needed to initialize WebAPI
		/// </summary>
		/// <param name="filters">Default WebAPI filter collection</param>
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
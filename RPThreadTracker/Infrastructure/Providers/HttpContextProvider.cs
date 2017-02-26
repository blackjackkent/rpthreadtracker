namespace RPThreadTracker.Infrastructure.Providers
{
	using System;
	using System.Web;
	using Filters;

	/// <summary>
	/// Adapted from https://codemagik.wordpress.com/2013/08/19/mocking-httpcontext-for-unit-tests/
	/// </summary>
	[ExcludeFromCoverage]
	public class HttpContextProvider
	{
		private static HttpContextBase _context;

		/// <summary>
		/// Gets wrapper for current HttpContext
		/// </summary>
		/// <value>
		/// HttpContextBase wrapper object
		/// </value>
		/// <exception cref="InvalidOperationException">Thrown if HttpContext not available</exception>
		public static HttpContextBase Current
		{
			get
			{
				if (_context != null)
				{
					return _context;
				}
				if (HttpContext.Current == null)
				{
					throw new InvalidOperationException("HttpContext not available");
				}
				return new HttpContextWrapper(HttpContext.Current);
			}
		}

		/// <summary>
		/// Sets value of current HttpContext
		/// </summary>
		/// <param name="context">HttpContextBase object to be wrapped</param>
		public static void SetCurrentContext(HttpContextBase context)
		{
			_context = context;
		}
	}
}
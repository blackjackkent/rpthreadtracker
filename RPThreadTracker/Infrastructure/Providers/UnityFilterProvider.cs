namespace RPThreadTracker.Infrastructure.Providers
{
	using System.Collections.Generic;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Filters;
	using Filters;
	using Microsoft.Practices.Unity;

	/// <summary>
	/// Provider which allows for WebAPI action filters to contain Unity-injected dependencies
	/// </summary>
	[ExcludeFromCoverage]
	public class UnityFilterProvider : IFilterProvider
	{
		private readonly ActionDescriptorFilterProvider _defaultProvider = new ActionDescriptorFilterProvider();
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityFilterProvider"/> class
		/// </summary>
		/// <param name="container">Unity container used by WebAPI</param>
		public UnityFilterProvider(IUnityContainer container)
		{
			_container = container;
		}

		/// <inheritdoc cref="IFilterProvider"/>
		public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
		{
			var attributes = _defaultProvider.GetFilters(configuration, actionDescriptor);

			foreach (var attr in attributes)
			{
				_container.BuildUp(attr.Instance.GetType(), attr.Instance);
			}
			return attributes;
		}
	}
}
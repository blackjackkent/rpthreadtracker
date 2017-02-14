namespace RPThreadTracker.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Web.Http.Dependencies;

	using Microsoft.Practices.Unity;

	/// <inheritdoc cref="IDependencyResolver"/>
	public class UnityResolver : IDependencyResolver
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnityResolver"/> class
		/// </summary>
		/// <param name="container">Unity container for dependency injection process</param>
		public UnityResolver(IUnityContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}
			_container = container;
		}

		/// <inheritdoc cref="IDependencyResolver"/>
		public IDependencyScope BeginScope()
		{
			var child = _container.CreateChildContainer();
			return new UnityResolver(child);
		}

		/// <inheritdoc cref="IDependencyResolver"/>
		public void Dispose()
		{
			try
			{
				_container.Dispose();
			}
			catch (TaskCanceledException e)
			{
				var ex = e;
			}
		}

		/// <inheritdoc cref="IDependencyResolver"/>
		public object GetService(Type serviceType)
		{
			try
			{
				return _container.Resolve(serviceType);
			}
			catch (ResolutionFailedException)
			{
				return null;
			}
		}

		/// <inheritdoc cref="IDependencyResolver"/>
		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return _container.ResolveAll(serviceType);
			}
			catch (ResolutionFailedException)
			{
				return new List<object>();
			}
		}
	}
}
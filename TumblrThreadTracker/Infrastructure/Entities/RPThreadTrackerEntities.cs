 // ReSharper disable once CheckNamespace
namespace TumblrThreadTracker.Infrastructure
{
	using Interfaces;

	/// <inheritdoc cref="IThreadTrackerContext"/>
	public partial class RpThreadTrackerEntities : IThreadTrackerContext
	{
		/// <inheritdoc cref="IThreadTrackerContext"/>
		public void Commit()
		{
			SaveChanges();
		}
	}
}
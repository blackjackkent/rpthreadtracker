 // ReSharper disable once CheckNamespace
namespace TumblrThreadTracker.Infrastructure
{
	using Interfaces;

	/// <inheritdoc cref="IThreadTrackerContext"/>
	public partial class RPThreadTrackerEntities : IThreadTrackerContext
	{
		/// <inheritdoc cref="IThreadTrackerContext"/>
		public void Commit()
		{
			SaveChanges();
		}
	}
}
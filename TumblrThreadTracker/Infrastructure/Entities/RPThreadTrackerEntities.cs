namespace TumblrThreadTracker.Infrastructure
{
	using TumblrThreadTracker.Interfaces;

	public partial class RPThreadTrackerEntities : IThreadTrackerContext
	{
		public void Commit()
		{
			SaveChanges();
		}
	}
}
namespace TumblrThreadTrackerTests.TestBuilders.Service
{
    public abstract class ServiceBuilder<T>
    {
        protected ServiceBuilder()
        {
        }

        public abstract T Build();
    }
}
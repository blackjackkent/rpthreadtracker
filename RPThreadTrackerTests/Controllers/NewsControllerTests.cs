namespace RPThreadTrackerTests.Controllers
{
	using System.Collections.Generic;
	using System.Web.Http.Results;
	using Helpers;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;
	using RPThreadTracker.Interfaces;
	using RPThreadTracker.Models.DomainModels.Threads;

	[TestFixture]
	internal class NewsControllerTests
	{
		private NewsController _newsController;
		private Mock<IThreadService> _threadService;
		private Mock<ITumblrClient> _tumblrClient;
		private Mock<IConfigurationService> _configurationService;

		[SetUp]
		public void Setup()
		{
			_threadService = new Mock<IThreadService>();
			_tumblrClient = new Mock<ITumblrClient>();
			_configurationService = new Mock<IConfigurationService>();
			_newsController = new NewsController(_threadService.Object, _tumblrClient.Object, _configurationService.Object);
		}

		[Test]
		public void Get_ReturnsNewsThreads()
		{
			// Arrange
			var list = new List<ThreadDto>
			{
				new ThreadBuilder()
					.WithUserBlogId(1)
					.BuildDto(),
				new ThreadBuilder()
					.WithUserBlogId(2)
					.BuildDto()
			};
			_threadService.Setup(t => t.GetNewsThreads(_tumblrClient.Object, _configurationService.Object)).Returns(list);

			// Act
			var result = _newsController.Get();

			// Assert
			Assert.That(result, Is.TypeOf<OkNegotiatedContentResult<IEnumerable<ThreadDto>>>());
			var response = result as OkNegotiatedContentResult<IEnumerable<ThreadDto>>;
			Assert.That(response, Is.Not.Null);
			Assert.That(response.Content, Is.EqualTo(list));
		}
	}
}

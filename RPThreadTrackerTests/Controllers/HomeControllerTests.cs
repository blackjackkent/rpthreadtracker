namespace RPThreadTrackerTests.Controllers
{
	using System.Web.Mvc;
	using NUnit.Framework;
	using RPThreadTracker.Controllers;

	[TestFixture]
	internal class HomeControllerTests
	{
		private HomeController _homeController;

		[SetUp]
		public void SetUp()
		{
			_homeController = new HomeController();
		}

		[Test]
		public void Index_ReturnsBaseView()
		{
			// Act
			var result = _homeController.Index();

			// Assert
			Assert.That(result, Is.TypeOf<ViewResult>());
			var response = result as ViewResult;
			Assert.That(response.ViewName, Is.EqualTo(string.Empty));
		}
	}
}

namespace RPThreadTrackerTests.Infrastructure.Filters
{
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Web;
	using System.Web.Http.Controllers;
	using Builders;
	using Moq;
	using NUnit.Framework;
	using RPThreadTracker.Infrastructure.Filters;
	using RPThreadTracker.Infrastructure.Providers;
	using RPThreadTracker.Interfaces;

	[TestFixture]
	internal class RedirectOnMaintenanceTests
	{
		private Mock<HttpContextBase> _contextBase;
		private Mock<HttpRequestBase> _requestBase;
		private HttpActionContext _httpActionContext;
		private Mock<IConfigurationService> _configurationService;

		[SetUp]
		public void Setup()
		{
			_configurationService = new Mock<IConfigurationService>();
			_contextBase = new Mock<HttpContextBase>();
			_requestBase = new Mock<HttpRequestBase>();
			_contextBase.Setup(b => b.Request).Returns(_requestBase.Object);
			_requestBase.Setup(b => b.UserHostAddress).Returns("1.1.1.1");
			HttpContextProvider.SetCurrentContext(_contextBase.Object);
			_httpActionContext = new HttpActionContext
			{
				ControllerContext = new HttpControllerContext
				{
					Request = new HttpRequestMessage()
				},
				Response = new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.Created
				}
			};
		}

		[Test]
		public void OnActionExecuting_MaintenanceModeAndNoAllowedIPs_Returns503()
		{
			// Arrange
			_configurationService.SetupGet(c => c.MaintenanceMode).Returns(true);
			_configurationService.SetupGet(c => c.AllowedMaintenanceIPs).Returns(new List<string>());

			// Act
			var filter = new RedirectOnMaintenance
			{
				ConfigurationService = _configurationService.Object
			};
			filter.OnActionExecuting(_httpActionContext);

			// Assert
			Assert.That(_httpActionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
		}

		[Test]
		public void OnActionExecuting_MaintenanceModeAndAllowedIPEqualsRequestIP_ReturnsRequestStatus()
		{
			// Arrange
			_configurationService.SetupGet(c => c.MaintenanceMode).Returns(true);
			_configurationService.SetupGet(c => c.AllowedMaintenanceIPs).Returns(new List<string> { "1.1.1.1" });

			// Act
			var filter = new RedirectOnMaintenance
			{
				ConfigurationService = _configurationService.Object
			};
			filter.OnActionExecuting(_httpActionContext);

			// Assert
			Assert.That(_httpActionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
		}

		[Test]
		public void OnActionExecuting_MaintenanceModeAndAllowedIPNotEqualsRequestIP_ReturnsRequestStatus()
		{
			// Arrange
			_configurationService.SetupGet(c => c.MaintenanceMode).Returns(true);
			_configurationService.SetupGet(c => c.AllowedMaintenanceIPs).Returns(new List<string> { "2.2.2.2" });

			// Act
			var filter = new RedirectOnMaintenance
			{
				ConfigurationService = _configurationService.Object
			};
			filter.OnActionExecuting(_httpActionContext);

			// Assert
			Assert.That(_httpActionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
		}

		[Test]
		public void OnActionExecuting_NotMaintenanceMode_ReturnsRequestStatus()
		{
			// Arrange
			_configurationService.SetupGet(c => c.MaintenanceMode).Returns(false);
			_configurationService.SetupGet(c => c.AllowedMaintenanceIPs).Returns(new List<string>());

			// Act
			var filter = new RedirectOnMaintenance
			{
				ConfigurationService = _configurationService.Object
			};
			filter.OnActionExecuting(_httpActionContext);

			// Assert
			Assert.That(_httpActionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
		}
	}
}

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using TumblrThreadTracker.Controllers;

namespace TumblrThreadTrackerTests.Controllers
{
    [TestFixture]
    public class SessionControllerTests
    {
        [Test]
        public void Null_Post_Should_Return_BadRequest_Response()
        {
            // Arrange
            var controller = new SessionController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            HttpResponseMessage response = controller.Post(null);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
using System.Linq;
using Moq;
using NUnit.Framework;
using RestSharp;
using TumblrThreadTracker.Infrastructure.Services;
using TumblrThreadTracker.Models.ServiceModels;
using TumblrThreadTrackerTests.TestBuilders.Domain;
using TumblrThreadTrackerTests.TestBuilders.Service;

namespace TumblrThreadTrackerTests.Infrastructure.Services
{
    [TestFixture]
    public class TumblrClientTests
    {
        [Test]
        public void GetPostRetrievesValidApiDataSuccessfully()
        {
            // Arrange
            var restClient = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<ServiceObject>>();
            var serviceObject = new ServiceObjectBuilder().Build();
            response.SetupGet(r => r.Data).Returns(serviceObject);
            restClient.Setup(c => c.Execute<ServiceObject>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var tumblrClient = new TumblrClient(restClient.Object);

            // Act
            var result = tumblrClient.GetPost("12345", "test");

            // Assert
            Assert.That(result, Is.EqualTo(serviceObject.response.posts.FirstOrDefault()));
        }

        [Test]
        public void GetPostRetrievesValidApiDataAfterCacheRefresh()
        {

            // Arrange
            var restClient = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<ServiceObject>>();
            var serviceObject = new ServiceObjectBuilder().Build();
            response.SetupSequence(r => r.Data).Returns(null).Returns(serviceObject);
            restClient.Setup(c => c.Execute<ServiceObject>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var tumblrClient = new TumblrClient(restClient.Object);

            // Act
            var result = tumblrClient.GetPost("12345", "test");

            // Assert
            Assert.That(result, Is.EqualTo(serviceObject.response.posts.FirstOrDefault()));
        }

        [Test]
        public void GetPostReturnsNullIfPostNotFoundAfterCacheRefresh()
        {
            // Arrange
            var restClient = new Mock<IRestClient>();
            var response = new Mock<IRestResponse<ServiceObject>>();
            response.SetupSequence(r => r.Data).Returns(null);
            restClient.Setup(c => c.Execute<ServiceObject>(It.IsAny<IRestRequest>())).Returns(response.Object);
            var tumblrClient = new TumblrClient(restClient.Object);

            // Act
            var result = tumblrClient.GetPost("12345", "test");

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
